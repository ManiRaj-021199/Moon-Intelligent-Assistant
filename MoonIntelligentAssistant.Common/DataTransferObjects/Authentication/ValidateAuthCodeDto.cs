namespace MoonIntelligentAssistant.Common;

public class ValidateAuthCodeDto
{
    #region Properties
    public string UserEmail { get; set; } = string.Empty;
    public string AuthenticationCode { get; set; } = string.Empty;
    #endregion
}