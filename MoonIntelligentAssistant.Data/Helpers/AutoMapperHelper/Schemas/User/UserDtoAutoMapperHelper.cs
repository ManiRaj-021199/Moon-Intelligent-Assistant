namespace MoonIntelligentAssistant.Data;

internal static class UserAutoMapperHelper
{
    #region Internals
    internal static User ToUser(this UserDto dtoUser)
    {
        return AutoMapperInitializer.Mapper.Map<User>(dtoUser);
    }

    internal static UserAuthentication ToUserAuthentication(this UserAuthenticationDto dtoUserAuthentication)
    {
        return AutoMapperInitializer.Mapper.Map<UserAuthentication>(dtoUserAuthentication);
    }
    #endregion
}