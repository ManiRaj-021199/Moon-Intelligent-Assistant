namespace MoonIntelligentAssistant.Logic;

internal class CommonDBContextActivities
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    internal CommonDBContextActivities(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Internals
    internal async Task PersistAsync()
    {
        await dbContext.SaveChangesAsync();
    }
    #endregion
}