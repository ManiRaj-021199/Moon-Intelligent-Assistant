namespace MoonIntelligentAssistant.Common;

public interface IUsersReader
{
    UserDto? GetByEmail(string strEmail);
}