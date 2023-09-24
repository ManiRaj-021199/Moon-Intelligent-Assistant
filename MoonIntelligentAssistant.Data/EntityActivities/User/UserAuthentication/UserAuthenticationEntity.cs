namespace MoonIntelligentAssistant.Data;

public class UserAuthenticationEntity : IUserAuthenticationEntity
{
    #region Properties
    public IUserAuthenticationReader UserAuthenticationReader { get; }
    public IUserAuthenticationWriter UserAuthenticationWriter { get; }
    #endregion

    #region Constructors
    public UserAuthenticationEntity(MoonIaContext dbContext)
    {
        this.UserAuthenticationReader = new UserAuthenticationReader(dbContext);
        this.UserAuthenticationWriter = new UserAuthenticationWriter(dbContext);
    }
    #endregion
}