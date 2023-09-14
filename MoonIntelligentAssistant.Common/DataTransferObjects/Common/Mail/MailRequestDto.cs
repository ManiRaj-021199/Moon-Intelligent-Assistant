namespace MoonIntelligentAssistant.Common;

public class MailRequestDto
{
    #region Properties
    public string ToMailAddress { get; set; } = string.Empty;
    public string MailTemplatePath { get; set; } = string.Empty;
    public MailType MailType { get; set; }
    #endregion
}