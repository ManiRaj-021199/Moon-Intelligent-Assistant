namespace MoonIntelligentAssistant.Common;

public interface IUserAuthenticationEntity
{
    IUserAuthenticationReader NonAuthUsersReader { get; }
    IUserAuthenticationWriter NonAuthUsersWriter { get; }
}