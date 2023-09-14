namespace MoonIntelligentAssistant.Data;

public static class UserAutoMapperHelper
{
    #region Publics
    public static AuthUser ToAuthUser(this AuthUserDto dtoAuthUser)
    {
        return AutoMapperInitializer.Mapper.Map<AuthUser>(dtoAuthUser);
    }

    public static NonAuthUser ToNonAuthUser(this NonAuthUserDto dtoNonAuthUser)
    {
        return AutoMapperInitializer.Mapper.Map<NonAuthUser>(dtoNonAuthUser);
    }
    #endregion
}