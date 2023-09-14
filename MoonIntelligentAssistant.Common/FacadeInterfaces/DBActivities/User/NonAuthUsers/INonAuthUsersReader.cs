namespace MoonIntelligentAssistant.Common;

public interface INonAuthUsersReader
{
    NonAuthUserDto? GetByEmail(string strEmail);
}