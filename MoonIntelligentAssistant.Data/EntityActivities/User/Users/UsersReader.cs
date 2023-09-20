namespace MoonIntelligentAssistant.Data;

public class UsersReader : IUsersReader
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public UsersReader(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public UserDto? GetByEmail(string strEmail)
    {
        User? user = dbContext.Users.FirstOrDefault(user => user.UserEmail == strEmail);
        dbContext.ClearChangeTracker();

        return user?.ToUserDto();
    }
    #endregion
}