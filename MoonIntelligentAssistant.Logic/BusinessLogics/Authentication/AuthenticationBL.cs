namespace MoonIntelligentAssistant.Logic;

internal class AuthenticationBL
{
    #region Fields
    private readonly AuthUsersReader readerAuthUsers;
    private readonly AuthUsersWriter writerAuthUsers;
    private readonly NonAuthUsersReader readerNonAuthUsers;
    private readonly NonAuthUsersWriter writerNonAuthUsers;
    private readonly CommonDBContextActivities dbContextActivities;
    #endregion

    #region Constructors
    internal AuthenticationBL(MoonIaContext dbContext)
    {
        readerAuthUsers = new AuthUsersReader(dbContext);
        writerAuthUsers = new AuthUsersWriter(dbContext);
        readerNonAuthUsers = new NonAuthUsersReader(dbContext);
        writerNonAuthUsers = new NonAuthUsersWriter(dbContext);
        dbContextActivities = new CommonDBContextActivities(dbContext);
    }
    #endregion

    #region Internals
    internal async Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister)
    {
        try
        {
            if(!dtoUserRegister.UserEmail.IsValidEmail()) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.InvalidEmail);

            AuthUser? authUser = readerAuthUsers.GetByEmail(dtoUserRegister.UserEmail);
            if(authUser != null) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.UserAlreadyRegistered);

            NonAuthUser? nonAuthUser = readerNonAuthUsers.GetByEmail(dtoUserRegister.UserEmail);
            string strAuthCode = IsAuthCodeExpired(nonAuthUser) ? RandomUtilitiesHelper.GenerateRandomString() : nonAuthUser!.AuthCode;

            if(nonAuthUser == null)
            {
                nonAuthUser = UserAutoMapperHelper.ToNonAuthUser(dtoUserRegister);
                nonAuthUser.AuthCode = strAuthCode;
                nonAuthUser.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                writerNonAuthUsers.Add(nonAuthUser);
            }
            else
            {
                nonAuthUser.AuthCode = strAuthCode;
                nonAuthUser.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                writerNonAuthUsers.Update(nonAuthUser);
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
            NonAuthUser? user = readerNonAuthUsers.GetByEmail(dtoValidateAuthCode.UserEmail);

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

            writerNonAuthUsers.Update(user);
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
            NonAuthUser? nonAuthUser = readerNonAuthUsers.GetByEmail(dtoUserRegisterPassword.UserEmail);

            if(nonAuthUser == null) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.UserNotRegistered);
            if(!nonAuthUser.IsAuthenticated) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.UserNotAuthenticated);

            PasswordHasherDto hashedPassword = PasswordHashingHelper.EncryptPassword(dtoUserRegisterPassword.Password);

            AuthUser authUser = UserAutoMapperHelper.ToAuthUser(nonAuthUser);
            authUser.UserId = 0;
            authUser.PasswordHash = hashedPassword.PasswordHash;
            authUser.PasswordSalt = hashedPassword.PasswordSalt;

            writerNonAuthUsers.Remove(nonAuthUser);
            writerAuthUsers.Add(authUser);

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
    private static bool IsAuthCodeExpired(NonAuthUser? user)
    {
        return user == null || DateTimeUtilitiesHelper.GetCurrentDateTime() - user.RegisterDate > new TimeSpan(0, 5, 0);
    }

    private static void SendUserRegisterAuthCode(NonAuthUser user, string strAuthCode)
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