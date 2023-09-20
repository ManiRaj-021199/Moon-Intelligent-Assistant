using AutoMapper;

namespace MoonIntelligentAssistant.Logic;

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
        return new MapperConfiguration(MapAuthenticationConfig);
    }

    private static void MapAuthenticationConfig(IProfileExpression config)
    {
        config.CreateMap<UserRegisterDto, UserAuthenticationDto>();
        config.CreateMap<UserAuthenticationDto, UserDto>();
    }
    #endregion
}