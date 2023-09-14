namespace MoonIntelligentAssistant.Data;

public class AuthUsersReader : IAuthUsersReader
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public AuthUsersReader(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;

        this.dbContext.TurnOffQueryTrackingBehaviour();
    }
    #endregion

    #region Publics
    public AuthUserDto? GetByEmail(string strEmail)
    {
        AuthUser? authUser = dbContext.AuthUsers.FirstOrDefault(user => user.UserEmail == strEmail);

        return authUser?.ToAuthUserDto();
    }
    #endregion
}