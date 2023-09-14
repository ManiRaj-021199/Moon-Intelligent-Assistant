namespace MoonIntelligentAssistant.Common;

public interface INonAuthUsersWriter
{
    void Add(NonAuthUserDto user);
    void Update(NonAuthUserDto user);
    void Remove(NonAuthUserDto user);
}