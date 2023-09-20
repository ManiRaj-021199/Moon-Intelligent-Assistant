namespace MoonIntelligentAssistant.Logic;

public static class AuthenticationAutoMapperHelper
{
    #region Publics
    public static UserAuthenticationDto ToUserAuthenticationDto(this UserRegisterDto dtoUserRegister)
    {
        return AutoMapperInitializer.Mapper.Map<UserAuthenticationDto>(dtoUserRegister);
    }

    public static UserDto ToUserDto(this UserAuthenticationDto dtoUserAuthentication)
    {
        return AutoMapperInitializer.Mapper.Map<UserDto>(dtoUserAuthentication);
    }
    #endregion
}