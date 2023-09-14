namespace MoonIntelligentAssistant.Data;

public class AuthUsersEntity : IAuthUsersEntity
{
    #region Properties
    public IAuthUsersReader AuthUsersReader { get; }
    public IAuthUsersWriter AuthUsersWriter { get; }
    #endregion

    #region Constructors
    public AuthUsersEntity(MoonIaContext dbContext)
    {
        this.AuthUsersReader = new AuthUsersReader(dbContext);
        this.AuthUsersWriter = new AuthUsersWriter(dbContext);
    }
    #endregion
}