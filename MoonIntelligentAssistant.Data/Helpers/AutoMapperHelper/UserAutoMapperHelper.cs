namespace MoonIntelligentAssistant.Data;

public static class UserAutoMapperHelper
{
    #region Publics
    public static NonAuthUser ToNonAuthUser(UserRegisterDto dtoUserRegister)
    {
        return AutoMapperInitializer.Mapper.Map<NonAuthUser>(dtoUserRegister);
    }

    public static AuthUser ToAuthUser(NonAuthUser user)
    {
        return AutoMapperInitializer.Mapper.Map<AuthUser>(user);
    }
    #endregion
}