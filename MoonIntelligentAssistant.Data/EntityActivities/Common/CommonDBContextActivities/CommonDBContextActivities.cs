namespace MoonIntelligentAssistant.Data;

public class CommonDBContextActivities : ICommonDBContextActivities
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public CommonDBContextActivities(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public async Task PersistAsync()
    {
        await dbContext.SaveChangesAsync();
    }
    #endregion
}