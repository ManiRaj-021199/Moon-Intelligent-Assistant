namespace MoonIntelligentAssistant.Data;

internal static class AutoMapperInitializer
{
    #region Constants
    internal static readonly Mapper Mapper = InitializeAutoMapper();
    #endregion

    #region Privates
    private static Mapper InitializeAutoMapper()
    {
        MapperConfiguration config = GetMapperConfiguration();
        Mapper mapper = new(config);

        return mapper;
    }

    private static MapperConfiguration GetMapperConfiguration()
    {
        return new MapperConfiguration(cfg =>
                                       {
                                           // User Entity
                                           cfg.CreateMap<UserRegisterDto, NonAuthUser>();
                                           cfg.CreateMap<NonAuthUser, AuthUser>();
                                       });
    }
    #endregion
}