namespace MoonIntelligentAssistant.Common;

public class PasswordHasherDto
{
    #region Properties
    public string PasswordHash { get; set; } = string.Empty;
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    #endregion
}