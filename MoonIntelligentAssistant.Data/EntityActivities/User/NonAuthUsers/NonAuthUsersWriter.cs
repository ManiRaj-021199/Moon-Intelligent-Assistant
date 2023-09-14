namespace MoonIntelligentAssistant.Data;

public class NonAuthUsersWriter : INonAuthUsersWriter
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public NonAuthUsersWriter(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public void Add(NonAuthUserDto user)
    {
        dbContext.NonAuthUsers.Add(user.ToNonAuthUser());
    }

    public void Update(NonAuthUserDto user)
    {
        dbContext.NonAuthUsers.Update(user.ToNonAuthUser());
    }

    public void Remove(NonAuthUserDto user)
    {
        dbContext.NonAuthUsers.Remove(user.ToNonAuthUser());
    }
    #endregion
}