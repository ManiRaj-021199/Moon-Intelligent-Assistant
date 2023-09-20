namespace MoonIntelligentAssistant.Common;

public interface IUsersEntity
{
    #region Properties
    public IUsersReader AuthUsersReader { get; }
    public IUsersWriter AuthUsersWriter { get; }
    #endregion
}