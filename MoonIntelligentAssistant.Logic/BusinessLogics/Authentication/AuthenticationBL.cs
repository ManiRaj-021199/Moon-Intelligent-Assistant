namespace MoonIntelligentAssistant.Logic;

internal class AuthenticationBL
{
    #region Fields
    private readonly IAuthUsersEntity entityAuthUsers;
    private readonly INonAuthUsersEntity entityNonAuthUsers;
    private readonly ICommonDBContextActivities dbContextActivities;
    #endregion

    #region Constructors
    public AuthenticationBL(IAuthUsersEntity entityAuthUsers, INonAuthUsersEntity entityNonAuthUsers, ICommonDBContextActivities dbContextActivities)
    {
        this.entityAuthUsers = entityAuthUsers;
        this.entityNonAuthUsers = entityNonAuthUsers;
        this.dbContextActivities = dbContextActivities;
    }
    #endregion

    #region Internals
    internal async Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister)
    {
        try
        {
            if(!dtoUserRegister.UserEmail.IsValidEmail()) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.InvalidEmail);

            AuthUserDto? authUser = entityAuthUsers.AuthUsersReader.GetByEmail(dtoUserRegister.UserEmail);
            if(authUser != null) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.UserAlreadyRegistered);

            NonAuthUserDto? nonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(dtoUserRegister.UserEmail);
            string strAuthCode = IsAuthCodeExpired(nonAuthUser) ? RandomUtilitiesHelper.GenerateRandomString() : nonAuthUser!.AuthCode;

            if(nonAuthUser == null)
            {
                nonAuthUser = dtoUserRegister.ToNonAuthUser();
                nonAuthUser.AuthCode = strAuthCode;
                nonAuthUser.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                entityNonAuthUsers.NonAuthUsersWriter.Add(nonAuthUser);
            }
            else
            {
                nonAuthUser.AuthCode = strAuthCode;
                nonAuthUser.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                entityNonAuthUsers.NonAuthUsersWriter.Update(nonAuthUser);
            }

            await dbContextActivities.PersistAsync();

            SendUserRegisterAuthCode(nonAuthUser, strAuthCode);

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.SendAuthenticationCode, dtoUserRegister);
        }
        catch
        {
            return ResponseBuilderHelper.BuildErrorResponse();
        }
    }

    internal async Task<BaseApiResponseDto> ValidateAuthCode(ValidateAuthCodeDto dtoValidateAuthCode)
    {
        try
        {
            NonAuthUserDto? user = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(dtoValidateAuthCode.UserEmail);

            if(user == null) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.UserNotRegistered);
            if(user.AuthCode != dtoValidateAuthCode.AuthenticationCode) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.WrongAuthCode);

            if(IsAuthCodeExpired(user))
            {
                await SendUserRegisterAuthCode(new UserRegisterDto
                                               {
                                                   UserEmail = dtoValidateAuthCode.UserEmail
                                               });

                return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.ExpiredAuthCode);
            }

            user.IsAuthenticated = true;

            entityNonAuthUsers.NonAuthUsersWriter.Update(user);
            await dbContextActivities.PersistAsync();

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.AuthCodeValidateSuccess);
        }
        catch
        {
            return ResponseBuilderHelper.BuildErrorResponse();
        }
    }

    internal async Task<BaseApiResponseDto> RegisterUser(UserRegisterPasswordDto dtoUserRegisterPassword)
    {
        try
        {
            NonAuthUserDto? nonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(dtoUserRegisterPassword.UserEmail);

            if(nonAuthUser == null) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.UserNotRegistered);
            if(!nonAuthUser.IsAuthenticated) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.UserNotAuthenticated);

            PasswordHasherDto hashedPassword = PasswordHashingHelper.EncryptPassword(dtoUserRegisterPassword.Password);

            AuthUserDto authUser = nonAuthUser.ToAuthUser();
            authUser.UserId = 0;
            authUser.PasswordHash = hashedPassword.PasswordHash;
            authUser.PasswordSalt = hashedPassword.PasswordSalt;

            entityNonAuthUsers.NonAuthUsersWriter.Remove(nonAuthUser);
            entityAuthUsers.AuthUsersWriter.Add(authUser);

            await dbContextActivities.PersistAsync();

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.UserRegisterSuccess);
        }
        catch
        {
            return ResponseBuilderHelper.BuildErrorResponse();
        }
    }
    #endregion

    #region Privates
    private static bool IsAuthCodeExpired(NonAuthUserDto? user)
    {
        return user == null || DateTimeUtilitiesHelper.GetCurrentDateTime() - user.RegisterDate > new TimeSpan(0, 5, 0);
    }

    private static void SendUserRegisterAuthCode(NonAuthUserDto user, string strAuthCode)
    {
        EmailHelper.SendEmail(new MailRequestDto
                              {
                                  ToMailAddress = user.UserEmail,
                                  MailType = MailType.UserRegisterAuthCode,
                                  MailTemplatePath = MailTemplatePathValues.UserRegisterAuthCode
                              },
                              new Dictionary<string, string>
                              {
                                  { "UserName", user.UserName },
                                  { "AuthenticationCode", strAuthCode }
                              });
    }
    #endregion
}