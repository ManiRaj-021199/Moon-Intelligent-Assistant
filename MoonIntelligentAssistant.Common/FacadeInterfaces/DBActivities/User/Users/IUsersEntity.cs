namespace MoonIntelligentAssistant.Common;

public interface IUsersEntity
{
    #region Properties
    public IUsersReader UsersReader { get; }
    public IUsersWriter UsersWriter { get; }
    #endregion
}