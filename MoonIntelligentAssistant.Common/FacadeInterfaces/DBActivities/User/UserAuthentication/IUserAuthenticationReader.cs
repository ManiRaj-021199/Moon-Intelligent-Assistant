namespace MoonIntelligentAssistant.Common;

public interface IUserAuthenticationReader
{
    UserAuthenticationDto? GetByEmail(string strEmail);
}