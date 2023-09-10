namespace MoonIntelligentAssistant.Logic;

internal class AuthUsersWriter
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    internal AuthUsersWriter(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Internals
    internal void Add(AuthUser user)
    {
        dbContext.AuthUsers.Add(user);
    }
    #endregion
}