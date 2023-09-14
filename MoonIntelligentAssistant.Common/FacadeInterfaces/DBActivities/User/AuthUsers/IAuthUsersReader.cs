namespace MoonIntelligentAssistant.Common;

public interface IAuthUsersReader
{
    AuthUserDto? GetByEmail(string strEmail);
}