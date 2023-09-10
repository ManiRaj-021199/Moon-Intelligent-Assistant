namespace MoonIntelligentAssistant.Logic;

internal class AuthenticationBL
{
    #region Fields
    private readonly NonAuthUsersReader readerNonAuthUsers;
    private readonly NonAuthUsersWriter writerNonAuthUsers;
    private readonly CommonDBContextActivities dbContextActivities;
    #endregion

    #region Constructors
    internal AuthenticationBL(MoonIaContext dbContext)
    {
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

            NonAuthUser? user = readerNonAuthUsers.GetByEmail(dtoUserRegister.UserEmail);

            string strAuthCode = IsAuthCodeExpired(user) ? RandomUtilitiesHelper.GenerateRandomString() : user!.AuthCode;

            if(user == null)
            {
                user = NonAuthUsersAutoMapperHelper.ToNonAuthUser(dtoUserRegister);
                user.AuthCode = strAuthCode;
                user.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                writerNonAuthUsers.Add(user);
            }
            else
            {
                user.AuthCode = strAuthCode;
                user.RegisterDate = DateTimeUtilitiesHelper.GetCurrentDateTime();

                writerNonAuthUsers.Update(user);
            }

            await dbContextActivities.PersistAsync();

            SendUserRegisterAuthCode(user, strAuthCode);

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