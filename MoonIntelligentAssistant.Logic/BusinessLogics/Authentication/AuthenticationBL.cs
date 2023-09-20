namespace MoonIntelligentAssistant.Logic;

internal class AuthenticationBL
{
    #region Fields
    private readonly IUsersEntity entityUsers;
    private readonly IUserAuthenticationEntity entityUserAuthentication;
    private readonly ICommonDBContextActivities dbContextCommonActivities;
    #endregion

    #region Constructors
    internal AuthenticationBL(IUsersEntity entityUsers, IUserAuthenticationEntity entityUserAuthentication, ICommonDBContextActivities dbContextCommonActivities)
    {
        this.entityUsers = entityUsers;
        this.entityUserAuthentication = entityUserAuthentication;
        this.dbContextCommonActivities = dbContextCommonActivities;
    }
    #endregion

    #region Internals
    internal async Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister)
    {
        try
        {
            ResponseBuilderHelper.objApiRequest = dtoUserRegister;

            if(!dtoUserRegister.UserEmail.IsValidEmail()) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.InvalidEmail);

            UserDto? dtoUser = entityUsers.AuthUsersReader.GetByEmail(dtoUserRegister.UserEmail);
            if(dtoUser != null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserAlreadyRegistered);

            UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(dtoUserRegister.UserEmail);
            string strAuthCode = IsAuthCodeExpired(dtoUserAuthentication) ? RandomUtilitiesHelper.GenerateRandomString() : dtoUserAuthentication!.AuthCode;

            if(dtoUserAuthentication == null)
            {
                dtoUserAuthentication = dtoUserRegister.ToUserAuthenticationDto();
                dtoUserAuthentication.AuthCode = strAuthCode;
                dtoUserAuthentication.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                entityUserAuthentication.NonAuthUsersWriter.Add(dtoUserAuthentication);
            }
            else
            {
                dtoUserAuthentication.AuthCode = strAuthCode;
                dtoUserAuthentication.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                entityUserAuthentication.NonAuthUsersWriter.Update(dtoUserAuthentication);
            }

            await dbContextCommonActivities.PersistAsync();

            SendUserRegisterAuthCode(dtoUserAuthentication, strAuthCode);

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.SendAuthenticationCode, dtoUserRegister);
        }
        catch(Exception exception)
        {
            return ResponseBuilderHelper.BuildErrorResponse(exception);
        }
    }

    internal async Task<BaseApiResponseDto> ValidateAuthCode(ValidateAuthCodeDto dtoValidateAuthCode)
    {
        try
        {
            ResponseBuilderHelper.objApiRequest = dtoValidateAuthCode;

            UserDto? dtoUser = entityUsers.AuthUsersReader.GetByEmail(dtoValidateAuthCode.UserEmail);
            if(dtoUser != null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserAlreadyRegistered);

            UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(dtoValidateAuthCode.UserEmail);
            if(dtoUserAuthentication == null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserNotRegistered);
            if(dtoUserAuthentication.AuthCode != dtoValidateAuthCode.AuthenticationCode) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.WrongAuthCode);

            if(IsAuthCodeExpired(dtoUserAuthentication))
            {
                await SendUserRegisterAuthCode(new UserRegisterDto
                                               {
                                                   UserEmail = dtoValidateAuthCode.UserEmail
                                               });

                return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.ExpiredAuthCode);
            }

            dtoUserAuthentication.IsAuthenticated = true;

            entityUserAuthentication.NonAuthUsersWriter.Update(dtoUserAuthentication);
            await dbContextCommonActivities.PersistAsync();

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.AuthCodeValidateSuccess);
        }
        catch(Exception exception)
        {
            return ResponseBuilderHelper.BuildErrorResponse(exception);
        }
    }

    internal async Task<BaseApiResponseDto> RegisterPassword(UserRegisterPasswordDto dtoUserRegisterPassword)
    {
        try
        {
            ResponseBuilderHelper.objApiRequest = dtoUserRegisterPassword;

            UserDto? dtoUser = entityUsers.AuthUsersReader.GetByEmail(dtoUserRegisterPassword.UserEmail);
            if(dtoUser != null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserAlreadyRegistered);

            UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(dtoUserRegisterPassword.UserEmail);
            if(dtoUserAuthentication == null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserNotRegistered);
            if(!dtoUserAuthentication.IsAuthenticated) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserNotAuthenticated);

            PasswordHasherDto hashedPassword = PasswordHashingHelper.EncryptPassword(dtoUserRegisterPassword.Password);

            dtoUser = dtoUserAuthentication.ToUserDto();
            dtoUser.UserId = 0;
            dtoUser.PasswordHash = hashedPassword.PasswordHash;
            dtoUser.PasswordSalt = hashedPassword.PasswordSalt;

            entityUserAuthentication.NonAuthUsersWriter.Remove(dtoUserAuthentication);
            entityUsers.AuthUsersWriter.Add(dtoUser);

            await dbContextCommonActivities.PersistAsync();

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.UserRegisterSuccess);
        }
        catch(Exception exception)
        {
            return ResponseBuilderHelper.BuildErrorResponse(exception);
        }
    }
    #endregion

    #region Privates
    private static bool IsAuthCodeExpired(UserAuthenticationDto? dtoUserAuthentication)
    {
        return dtoUserAuthentication == null || DateTimeUtilitiesHelper.GetCurrentDateTime() - dtoUserAuthentication.RegisterDate > new TimeSpan(0, 5, 0);
    }

    private static void SendUserRegisterAuthCode(UserAuthenticationDto dtoUserAuthentication, string strAuthCode)
    {
        EmailHelper.SendEmail(new MailRequestDto
                              {
                                  ToMailAddress = dtoUserAuthentication.UserEmail,
                                  MailType = MailType.UserRegisterAuthCode,
                                  MailTemplatePath = MailTemplatePathValues.UserRegisterAuthCode
                              },
                              new Dictionary<string, string>
                              {
                                  { UserRegisterAuthCodeValues.UserName, dtoUserAuthentication.UserName },
                                  { UserRegisterAuthCodeValues.AuthenticationCode, strAuthCode }
                              });
    }
    #endregion
}