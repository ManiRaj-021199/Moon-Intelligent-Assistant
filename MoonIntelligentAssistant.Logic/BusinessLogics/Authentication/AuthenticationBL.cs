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
            
            bool bIsNeedToGenerateAuthCode = user == null || DateTimeUtilitiesHelper.GetCurrentDateTime() - user.RegisterDate > new TimeSpan(0, 5, 0);
            string strAuthCode = bIsNeedToGenerateAuthCode ? RandomUtilitiesHelper.GenerateRandomString() : user!.AuthCode;

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
            
            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.SendAuthenticationCode, dtoUserRegister);
        }
        catch
        {
            return ResponseBuilderHelper.BuildErrorResponse();
        }
    }
    #endregion
}