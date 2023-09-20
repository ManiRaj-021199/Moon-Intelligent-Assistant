namespace MoonIntelligentAssistant.Data;

public class UserAuthenticationWriter : IUserAuthenticationWriter
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public UserAuthenticationWriter(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public void Add(UserAuthenticationDto userAuthentication)
    {
        dbContext.UserAuthentications.Add(userAuthentication.ToUserAuthentication());
    }

    public void Update(UserAuthenticationDto userAuthentication)
    {
        dbContext.UserAuthentications.Update(userAuthentication.ToUserAuthentication());
    }

    public void Remove(UserAuthenticationDto userAuthentication)
    {
        dbContext.UserAuthentications.Remove(userAuthentication.ToUserAuthentication());
    }
    #endregion
}