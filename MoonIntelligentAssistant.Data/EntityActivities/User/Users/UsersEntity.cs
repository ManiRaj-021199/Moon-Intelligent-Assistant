namespace MoonIntelligentAssistant.Data;

public class UsersEntity : IUsersEntity
{
    #region Properties
    public IUsersReader AuthUsersReader { get; }
    public IUsersWriter AuthUsersWriter { get; }
    #endregion

    #region Constructors
    public UsersEntity(MoonIaContext dbContext)
    {
        this.AuthUsersReader = new UsersReader(dbContext);
        this.AuthUsersWriter = new UsersWriter(dbContext);
    }
    #endregion
}