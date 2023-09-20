namespace MoonIntelligentAssistant.Data;

public class UserAuthenticationEntity : IUserAuthenticationEntity
{
    #region Properties
    public IUserAuthenticationReader NonAuthUsersReader { get; }
    public IUserAuthenticationWriter NonAuthUsersWriter { get; }
    #endregion

    #region Constructors
    public UserAuthenticationEntity(MoonIaContext dbContext)
    {
        this.NonAuthUsersReader = new UserAuthenticationReader(dbContext);
        this.NonAuthUsersWriter = new UserAuthenticationWriter(dbContext);
    }
    #endregion
}