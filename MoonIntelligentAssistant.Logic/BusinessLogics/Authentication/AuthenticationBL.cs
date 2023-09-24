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

            UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.UserAuthenticationReader.GetByEmail(dtoUserRegister.UserEmail);
            string strAuthCode = IsAuthCodeExpired(dtoUserAuthentication) ? RandomUtilitiesHelper.GenerateRandomString() : dtoUserAuthentication!.AuthCode;

            if(dtoUserAuthentication == null)
            {
                dtoUserAuthentication = dtoUserRegister.ToUserAuthenticationDto();
                dtoUserAuthentication.AuthCode = strAuthCode;
                dtoUserAuthentication.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                entityUserAuthentication.UserAuthenticationWriter.Add(dtoUserAuthentication);
            }
            else
            {
                dtoUserAuthentication.AuthCode = strAuthCode;
                dtoUserAuthentication.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                entityUserAuthentication.UserAuthenticationWriter.Update(dtoUserAuthentication);
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
            
            UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.UserAuthenticationReader.GetByEmail(dtoValidateAuthCode.UserEmail);
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

            entityUserAuthentication.UserAuthenticationWriter.Update(dtoUserAuthentication);
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

            UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.UserAuthenticationReader.GetByEmail(dtoUserRegisterPassword.UserEmail);
            if(dtoUserAuthentication == null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserNotRegistered);
            if(!dtoUserAuthentication.IsAuthenticated) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserNotAuthenticated);

            PasswordHasherDto hashedPassword = PasswordHashingHelper.EncryptPassword(dtoUserRegisterPassword.Password);

            UserDto dtoUser = dtoUserAuthentication.ToUserDto();
            dtoUser.UserId = 0;
            dtoUser.PasswordHash = hashedPassword.PasswordHash;
            dtoUser.PasswordSalt = hashedPassword.PasswordSalt;
            dtoUser.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

            entityUserAuthentication.UserAuthenticationWriter.Remove(dtoUserAuthentication);
            await dbContextCommonActivities.PersistAsync();

            UserDto? dtoUserNeedToUpdate = entityUsers.UsersReader.GetByEmail(dtoUserRegisterPassword.UserEmail);

            if(dtoUserNeedToUpdate == null) entityUsers.UsersWriter.Add(dtoUser);
            else
            {
                dtoUserNeedToUpdate.PasswordHash = hashedPassword.PasswordHash;
                dtoUserNeedToUpdate.PasswordSalt = hashedPassword.PasswordSalt;
                dtoUserNeedToUpdate.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                entityUsers.UsersWriter.Update(dtoUserNeedToUpdate);
            }

            await dbContextCommonActivities.PersistAsync();

            return ResponseBuilderHelper.BuildSuccessResponse(dtoUserNeedToUpdate == null ? AuthenticationSuccessMessages.UserRegisterSuccess : AuthenticationSuccessMessages.PasswordUpdateSuccess);
        }
        catch(Exception exception)
        {
            return ResponseBuilderHelper.BuildErrorResponse(exception);
        }
    }

    internal async Task<BaseApiResponseDto> ValidateUserLogin(UserRegisterPasswordDto dtoUserRegisterPassword)
    {
        try
        {
            ResponseBuilderHelper.objApiRequest = dtoUserRegisterPassword;
            
            UserDto? dtoUser = entityUsers.UsersReader.GetByEmail(dtoUserRegisterPassword.UserEmail);
            if(dtoUser == null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserNotRegistered);

            UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.UserAuthenticationReader.GetByEmail(dtoUserRegisterPassword.UserEmail);
            if(dtoUserAuthentication != null) return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.UserNotAuthenticated);

            if(dtoUser.FailedLoginCount >= AuthenticationConstantValues.MAXIMUM_FAILED_LOGIN_ATTEMPTS_ALLOWED && DateTimeUtilitiesHelper.GetCurrentDateTime() - dtoUser.RegisterDate < new TimeSpan(1, 0, 0))
            {
                return ResponseBuilderHelper.BuildWarningResponse(AuthenticationErrorMessages.FailedAttemptsExceeds);
            }

            bool bIsLoginSuccess = PasswordHashingHelper.VerifyHashedPassword(dtoUserRegisterPassword.Password, new PasswordHasherDto
                                                                                                                {
                                                                                                                    PasswordHash = dtoUser.PasswordHash,
                                                                                                                    PasswordSalt = dtoUser.PasswordSalt
                                                                                                                });

            if(!bIsLoginSuccess)
            {
                dtoUser.FailedLoginCount += 1;
                entityUsers.UsersWriter.Update(dtoUser);
                await dbContextCommonActivities.PersistAsync();

                return ResponseBuilderHelper.BuildWarningResponse(string.Format(AuthenticationErrorMessages.WrongPassword, AuthenticationConstantValues.MAXIMUM_FAILED_LOGIN_ATTEMPTS_ALLOWED - dtoUser.FailedLoginCount));
            }
            
            dtoUser.FailedLoginCount = 0;
            entityUsers.UsersWriter.Update(dtoUser);
            await dbContextCommonActivities.PersistAsync();

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.UserLoginSuccess);
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