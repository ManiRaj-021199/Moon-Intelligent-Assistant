namespace MoonIntelligentAssistant.Data;

internal static class UserDtoAutoMapperHelper
{
    #region Internals
    internal static UserDto ToUserDto(this User user)
    {
        return AutoMapperInitializer.Mapper.Map<UserDto>(user);
    }

    internal static UserAuthenticationDto ToUserAuthenticationDto(this UserAuthentication userAuthentication)
    {
        return AutoMapperInitializer.Mapper.Map<UserAuthenticationDto>(userAuthentication);
    }
    #endregion
}