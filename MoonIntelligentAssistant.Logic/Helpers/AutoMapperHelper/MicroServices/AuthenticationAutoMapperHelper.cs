namespace MoonIntelligentAssistant.Logic;

public static class AuthenticationAutoMapperHelper
{
    #region Publics
    public static NonAuthUserDto ToNonAuthUser(this UserRegisterDto dtoUserRegister)
    {
        return AutoMapperInitializer.Mapper.Map<NonAuthUserDto>(dtoUserRegister);
    }

    public static AuthUserDto ToAuthUser(this NonAuthUserDto dtoNonAuthUser)
    {
        return AutoMapperInitializer.Mapper.Map<AuthUserDto>(dtoNonAuthUser);
    }
    #endregion
}