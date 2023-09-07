namespace MoonIntelligentAssistant.Logic;

public class AuthenticationFacade : IAuthenticationFacade
{
    #region Fields
    private readonly AuthenticationBL blAuthentication;
    #endregion

    #region Constructors
    public AuthenticationFacade(MoonIaContext dbContext)
    {
        blAuthentication = new AuthenticationBL(dbContext);
    }
    #endregion

    #region Publics
    public async Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister)
    {
        return await blAuthentication.SendUserRegisterAuthCode(dtoUserRegister);
    }
    #endregion
}