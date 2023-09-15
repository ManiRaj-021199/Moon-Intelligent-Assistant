namespace MoonIntelligentAssistant.Data;

internal static class UserAutoMapperHelper
{
    #region Internals
    internal static AuthUser ToAuthUser(this AuthUserDto dtoAuthUser)
    {
        return AutoMapperInitializer.Mapper.Map<AuthUser>(dtoAuthUser);
    }

    internal static NonAuthUser ToNonAuthUser(this NonAuthUserDto dtoNonAuthUser)
    {
        return AutoMapperInitializer.Mapper.Map<NonAuthUser>(dtoNonAuthUser);
    }
    #endregion
}