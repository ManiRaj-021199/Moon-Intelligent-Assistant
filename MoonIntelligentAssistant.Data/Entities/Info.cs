namespace MoonIntelligentAssistant.Data.Entities;

public partial class Info
{
    #region Properties
    public int LogId { get; set; }
    public string ApiName { get; set; } = null!;
    public string ApiSeverity { get; set; } = null!;
    public string ApiRequest { get; set; } = null!;
    public string ApiResponse { get; set; } = null!;
    public DateTime LogDate { get; set; }
    #endregion
}