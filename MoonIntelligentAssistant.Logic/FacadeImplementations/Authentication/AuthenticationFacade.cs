namespace MoonIntelligentAssistant.Logic;

public class AuthenticationFacade : IAuthenticationFacade
{
    #region Fields
    private readonly AuthenticationBL blAuthentication;
    #endregion

    #region Constructors
    public AuthenticationFacade(ILogEntity entityLog, IUsersEntity entityUsers, IUserAuthenticationEntity entityUserAuthentication, ICommonDBContextActivities dbContextCommonActivities)
    {
        blAuthentication = new AuthenticationBL(entityUsers, entityUserAuthentication, dbContextCommonActivities);

        ResponseBuilderHelper.entityLog = entityLog;
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

    public async Task<BaseApiResponseDto> RegisterPassword(UserRegisterPasswordDto dtoUserRegisterPassword)
    {
        return await blAuthentication.RegisterPassword(dtoUserRegisterPassword);
    }

    public async Task<BaseApiResponseDto> ValidateUserLogin(UserRegisterPasswordDto dtoUserRegisterPassword)
    {
        return await blAuthentication.ValidateUserLogin(dtoUserRegisterPassword);
    }
    #endregion
}