using Microsoft.AspNetCore.Mvc;

namespace Authentication;

[ApiController]
[Route("/[controller]")]
public class AuthenticationController : ControllerBase
{
    #region Publics
    [HttpPost]
    [Route("[action]")]
    public string LoginUser()
    {
        return "Login Success";
    }
    #endregion
}