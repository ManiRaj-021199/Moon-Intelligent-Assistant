namespace MoonIntelligentAssistant.Logic;

internal class AuthenticationBL
{
    #region Fields
    private readonly NonAuthUsersReader readerNonAuthUsers;
    private readonly NonAuthUsersWriter writerNonAuthUsers;
    #endregion

    #region Constructors
    internal AuthenticationBL(MoonIaContext dbContext)
    {
        readerNonAuthUsers = new NonAuthUsersReader(dbContext);
        writerNonAuthUsers = new NonAuthUsersWriter(dbContext);
    }
    #endregion

    #region Internals
    internal async Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister)
    {
        try
        {
            if(!dtoUserRegister.UserEmail.IsValidEmail()) return ResponseBuilderHelper.BuildErrorResponse(AuthenticationErrorMessages.InvalidEmail);
            
            NonAuthUser? user = readerNonAuthUsers.GetUserByEmail(dtoUserRegister.UserEmail);

            string strAuthCode = RandomUtilitiesHelper.GenerateRandomString();
            DateTime dtCurrentDateTime = DateTimeUtilitiesHelper.GetCurrentDateTime();

            if(user == null)
            {
                user = NonAuthUsersAutoMapperHelper.ToNonAuthUser(dtoUserRegister);
                user.AuthCode = strAuthCode;
                user.RegisterDate = dtCurrentDateTime;

                await writerNonAuthUsers.AddNewUser(user);
            }
            else
            {
                user.AuthCode = strAuthCode;
                user.RegisterDate = dtCurrentDateTime;

                await writerNonAuthUsers.UpdateUser(user);
            }

            return ResponseBuilderHelper.BuildSuccessResponse(AuthenticationSuccessMessages.SendAuthenticationCode, dtoUserRegister);
        }
        catch
        {
            return ResponseBuilderHelper.BuildErrorResponse();
        }
    }
    #endregion
}