namespace MoonIntelligentAssistant.Test;

[TestClass]
public class MoonIATestBase
{
    #region Fields
    protected readonly IAuthUsersEntity entityAuthUsers;
    protected readonly INonAuthUsersEntity entityNonAuthUsers;

    protected readonly ICommonDBContextActivities activityDbContext;

    protected readonly ILogEntity entityLog;
    protected readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public MoonIATestBase()
    {
        dbContext = new MoonIaContext(BaseTestConst.DB_CONNECTION_STRING);

        entityAuthUsers = new AuthUsersEntity(dbContext);
        entityNonAuthUsers = new NonAuthUsersEntity(dbContext);

        activityDbContext = new CommonDBContextActivities(dbContext);

        entityLog = new LogEntity(dbContext);
    }
    #endregion
}