namespace MoonIntelligentAssistant.Data.Entities;

public class User
{
    #region Properties
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime RegisterDate { get; set; }
    #endregion
}