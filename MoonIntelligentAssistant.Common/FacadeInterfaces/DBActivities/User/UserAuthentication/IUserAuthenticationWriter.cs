namespace MoonIntelligentAssistant.Common;

public interface IUserAuthenticationWriter
{
    void Add(UserAuthenticationDto user);
    void Update(UserAuthenticationDto user);
    void Remove(UserAuthenticationDto user);
}