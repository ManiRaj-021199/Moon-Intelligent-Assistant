using Microsoft.AspNetCore.Mvc;
using MoonIntelligentAssistant.Common;

namespace Authentication;

[ApiController]
[Route("/[controller]")]
public class AuthenticationController : ControllerBase
{
    #region Fields
    private readonly IAuthenticationFacade facadeAuthentication;
    #endregion

    #region Constructors
    public AuthenticationController(IAuthenticationFacade facadeAuthentication)
    {
        this.facadeAuthentication = facadeAuthentication;
    }
    #endregion

    #region Publics
    [HttpPost]
    [Route("[action]")]
    public async Task<BaseApiResponseDto> SendUserRegisterAuthCode(UserRegisterDto dtoUserRegister)
    {
        return await facadeAuthentication.SendUserRegisterAuthCode(dtoUserRegister);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<BaseApiResponseDto> ValidateAuthCode(ValidateAuthCodeDto dtoValidateAuthCode)
    {
        return await facadeAuthentication.ValidateAuthCode(dtoValidateAuthCode);
    }
    #endregion
}