namespace MoonIntelligentAssistant.Common;

public interface IAuthenticationFacade
{
    public Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister);
    public Task<BaseApiResponseDto> ValidateAuthCode(ValidateAuthCodeDto dtoValidateAuthCode);
}