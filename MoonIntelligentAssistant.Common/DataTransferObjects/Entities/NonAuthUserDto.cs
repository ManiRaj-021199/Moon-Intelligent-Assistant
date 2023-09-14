namespace MoonIntelligentAssistant.Common;

public class NonAuthUserDto
{
    #region Properties
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string AuthCode { get; set; } = null!;
    public bool IsAuthenticated { get; set; }
    public DateTime RegisterDate { get; set; }
    #endregion
}