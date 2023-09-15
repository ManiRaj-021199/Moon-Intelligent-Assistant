namespace MoonIntelligentAssistant.Common;

public class ErrorDto
{
    #region Properties
    public int LogId { get; set; }
    public string ApiName { get; set; } = null!;
    public string ApiRequest { get; set; } = null!;
    public string Exception { get; set; } = null!;
    public DateTime LogDate { get; set; }
    #endregion
}