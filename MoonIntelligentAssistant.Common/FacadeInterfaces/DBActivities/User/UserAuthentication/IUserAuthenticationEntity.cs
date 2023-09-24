namespace MoonIntelligentAssistant.Common;

public interface IUserAuthenticationEntity
{
    IUserAuthenticationReader UserAuthenticationReader { get; }
    IUserAuthenticationWriter UserAuthenticationWriter { get; }
}