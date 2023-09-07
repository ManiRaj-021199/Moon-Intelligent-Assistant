namespace MoonIntelligentAssistant.Common;

public class PasswordHasherDto
{
    #region Properties
    public string? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    #endregion
}