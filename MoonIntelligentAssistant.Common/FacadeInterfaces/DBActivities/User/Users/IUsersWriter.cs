namespace MoonIntelligentAssistant.Common;

public interface IUsersWriter
{
    void Add(UserDto dtoUser);
    void Update(UserDto dtoUser);
}