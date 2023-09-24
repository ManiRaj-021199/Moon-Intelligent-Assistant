namespace MoonIntelligentAssistant.Data;

public class UsersEntity : IUsersEntity
{
    #region Properties
    public IUsersReader UsersReader { get; }
    public IUsersWriter UsersWriter { get; }
    #endregion

    #region Constructors
    public UsersEntity(MoonIaContext dbContext)
    {
        this.UsersReader = new UsersReader(dbContext);
        this.UsersWriter = new UsersWriter(dbContext);
    }
    #endregion
}