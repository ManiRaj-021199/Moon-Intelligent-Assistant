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
    internal async Task AddNewUser(NonAuthUser user)
    {
        dbContext.NonAuthUsers.Add(user);

        await dbContext.SaveChangesAsync();
    }

    internal async Task UpdateUser(NonAuthUser user)
    {
        dbContext.NonAuthUsers.Update(user);

        await dbContext.SaveChangesAsync();
    }
    #endregion
}