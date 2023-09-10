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

    public async Task<BaseApiResponseDto> ValidateAuthCode(ValidateAuthCodeDto dtoValidateAuthCode)
    {
        return await blAuthentication.ValidateAuthCode(dtoValidateAuthCode);
    }

    public async Task<BaseApiResponseDto> RegisterUser(UserRegisterPasswordDto dtoUserRegisterPassword)
    {
        return await blAuthentication.RegisterUser(dtoUserRegisterPassword);
    }
    #endregion
}