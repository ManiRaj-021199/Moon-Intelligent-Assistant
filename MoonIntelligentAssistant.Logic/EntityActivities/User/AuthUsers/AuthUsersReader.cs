namespace MoonIntelligentAssistant.Logic;

internal class AuthUsersReader
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    internal AuthUsersReader(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Internals
    internal AuthUser? GetByEmail(string strEmail)
    {
        return dbContext.AuthUsers.FirstOrDefault(user => user.UserEmail == strEmail);
    }
    #endregion
}