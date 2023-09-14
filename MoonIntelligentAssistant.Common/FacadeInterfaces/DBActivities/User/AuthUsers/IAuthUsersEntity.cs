namespace MoonIntelligentAssistant.Common;

public interface IAuthUsersEntity
{
    #region Properties
    public IAuthUsersReader AuthUsersReader { get; }
    public IAuthUsersWriter AuthUsersWriter { get; }
    #endregion
}