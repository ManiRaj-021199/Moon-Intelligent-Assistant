namespace MoonIntelligentAssistant.Data;

public class NonAuthUsersAutoMapperHelper
{
    #region Publics
    public static NonAuthUser ToNonAuthUser(UserRegisterDto dtoUserRegister)
    {
        return AutoMapperInitializer.Mapper.Map<NonAuthUser>(dtoUserRegister);
    }
    #endregion
}