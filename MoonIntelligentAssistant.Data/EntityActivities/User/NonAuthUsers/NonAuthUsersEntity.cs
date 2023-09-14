namespace MoonIntelligentAssistant.Data;

public class NonAuthUsersEntity : INonAuthUsersEntity
{
    #region Properties
    public INonAuthUsersReader NonAuthUsersReader { get; }
    public INonAuthUsersWriter NonAuthUsersWriter { get; }
    #endregion

    #region Constructors
    public NonAuthUsersEntity(MoonIaContext dbContext)
    {
        this.NonAuthUsersReader = new NonAuthUsersReader(dbContext);
        this.NonAuthUsersWriter = new NonAuthUsersWriter(dbContext);
    }
    #endregion
}