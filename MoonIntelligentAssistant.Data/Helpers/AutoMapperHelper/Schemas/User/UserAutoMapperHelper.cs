namespace MoonIntelligentAssistant.Data;

public static class UserDtoAutoMapperHelper
{
    #region Publics
    public static AuthUserDto ToAuthUserDto(this AuthUser authUser)
    {
        return AutoMapperInitializer.Mapper.Map<AuthUserDto>(authUser);
    }

    public static NonAuthUserDto ToNonAuthUserDto(this NonAuthUser nonAuthUser)
    {
        return AutoMapperInitializer.Mapper.Map<NonAuthUserDto>(nonAuthUser);
    }
    #endregion
}