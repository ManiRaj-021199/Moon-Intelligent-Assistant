namespace MoonIntelligentAssistant.Common;

public interface INonAuthUsersEntity
{
    INonAuthUsersReader NonAuthUsersReader { get; }
    INonAuthUsersWriter NonAuthUsersWriter { get; }
}