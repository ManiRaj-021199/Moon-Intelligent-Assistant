namespace MoonIntelligentAssistant.Common;

public interface IAuthenticationFacade
{
    Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister);
    Task<BaseApiResponseDto> ValidateAuthCode(ValidateAuthCodeDto dtoValidateAuthCode);
    Task<BaseApiResponseDto> RegisterUser(UserRegisterPasswordDto dtoUserRegisterPassword);
}