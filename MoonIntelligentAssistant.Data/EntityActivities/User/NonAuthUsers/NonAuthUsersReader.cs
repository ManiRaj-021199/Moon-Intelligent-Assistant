namespace MoonIntelligentAssistant.Data;

public class NonAuthUsersReader : INonAuthUsersReader
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public NonAuthUsersReader(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;

        this.dbContext.TurnOffQueryTrackingBehaviour();
    }
    #endregion

    #region Publics
    public NonAuthUserDto? GetByEmail(string strEmail)
    {
        NonAuthUser? nonAuthUser = dbContext.NonAuthUsers.FirstOrDefault(x => x.UserEmail == strEmail);

        return nonAuthUser?.ToNonAuthUserDto();
    }
    #endregion
}