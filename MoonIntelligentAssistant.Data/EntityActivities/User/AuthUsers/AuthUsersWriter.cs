namespace MoonIntelligentAssistant.Data;

public class AuthUsersWriter : IAuthUsersWriter
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public AuthUsersWriter(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public void Add(AuthUserDto dtoAuthUser)
    {
        dbContext.AuthUsers.Add(dtoAuthUser.ToAuthUser());
    }
    #endregion
}