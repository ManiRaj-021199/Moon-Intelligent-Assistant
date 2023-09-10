namespace MoonIntelligentAssistant.Logic;

internal class NonAuthUsersReader
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    internal NonAuthUsersReader(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Internals
    internal NonAuthUser? GetByEmail(string strEmail)
    {
        return dbContext.NonAuthUsers.FirstOrDefault(x => x.UserEmail == strEmail);
    }
    #endregion
}