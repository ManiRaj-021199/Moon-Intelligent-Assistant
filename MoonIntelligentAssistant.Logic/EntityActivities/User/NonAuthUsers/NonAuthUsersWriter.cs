namespace MoonIntelligentAssistant.Logic;

internal class NonAuthUsersWriter
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    internal NonAuthUsersWriter(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Internals
    internal void Add(NonAuthUser user)
    {
        dbContext.NonAuthUsers.Add(user);
    }

    internal void Update(NonAuthUser user)
    {
        dbContext.NonAuthUsers.Update(user);
    }

    internal void Remove(NonAuthUser user)
    {
        dbContext.NonAuthUsers.Remove(user);
    }
    #endregion
}