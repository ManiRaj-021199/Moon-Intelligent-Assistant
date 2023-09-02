namespace MoonIntelligentAssistant.Data.Entities;

public class User
{
    #region Properties
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime RegisterDate { get; set; }
    #endregion
}