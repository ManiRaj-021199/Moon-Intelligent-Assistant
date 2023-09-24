namespace MoonIntelligentAssistant.Test;

[TestClass]
public class MoonIATestBase
{
    #region Fields
    protected readonly IUsersEntity entityUsers;
    protected readonly IUserAuthenticationEntity entityUserAuthentication;

    protected readonly ICommonDBContextActivities dbContextCommonActivities;

    protected readonly ILogEntity entityLog;
    protected readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public MoonIATestBase()
    {
        dbContext = new MoonIaContext(BaseTestConst.DB_CONNECTION_STRING);

        entityUsers = new UsersEntity(dbContext);
        entityUserAuthentication = new UserAuthenticationEntity(dbContext);

        dbContextCommonActivities = new CommonDBContextActivities(dbContext);

        entityLog = new LogEntity(dbContext);
    }
    #endregion
}