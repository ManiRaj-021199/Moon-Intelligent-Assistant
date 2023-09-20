namespace MoonIntelligentAssistant.Data;

public class UsersWriter : IUsersWriter
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public UsersWriter(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public void Add(UserDto dtoUser)
    {
        dbContext.Users.Add(dtoUser.ToUser());
    }
    #endregion
}