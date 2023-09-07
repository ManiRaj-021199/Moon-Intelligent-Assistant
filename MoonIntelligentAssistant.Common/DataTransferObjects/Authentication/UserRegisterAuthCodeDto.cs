namespace MoonIntelligentAssistant.Common;

public class UserRegisterAuthCodeDto
{
    #region Properties
    public string UserEmail { get; set; } = string.Empty;
    public string AuthenticationCode { get; set; } = string.Empty;
    #endregion
}