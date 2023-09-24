namespace MoonIntelligentAssistant.Data;

public class UserAuthenticationReader : IUserAuthenticationReader
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public UserAuthenticationReader(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public UserAuthenticationDto? GetByEmail(string strEmail)
    {
        UserAuthentication? userAuthentication = dbContext.UserAuthentications.FirstOrDefault(x => x.UserEmail == strEmail);
        dbContext.ClearChangeTracker();

        return userAuthentication?.ToUserAuthenticationDto();
    }
    #endregion
}