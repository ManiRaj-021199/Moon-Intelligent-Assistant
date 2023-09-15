namespace MoonIntelligentAssistant.Data;

internal static class UserDtoAutoMapperHelper
{
    #region Internals
    internal static AuthUserDto ToAuthUserDto(this AuthUser authUser)
    {
        return AutoMapperInitializer.Mapper.Map<AuthUserDto>(authUser);
    }

    internal static NonAuthUserDto ToNonAuthUserDto(this NonAuthUser nonAuthUser)
    {
        return AutoMapperInitializer.Mapper.Map<NonAuthUserDto>(nonAuthUser);
    }
    #endregion
}