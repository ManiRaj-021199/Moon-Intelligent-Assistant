using Microsoft.AspNetCore.Mvc;

namespace Resources;

[ApiController]
[Route("/[controller]")]
public class PagesController
{
    #region Publics
    [HttpGet]
    [Route("[action]")]
    public string GetAllAvailablePages()
    {
        return "Hello, World!";
    }
    #endregion
}