﻿namespace MoonIntelligentAssistant.Data;

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
        return new MapperConfiguration(config =>
                                       {
                                           // User Schema
                                           MapSchemaConfig(config);

                                           // User Schema Dto
                                           MapSchemaDtoConfig(config);
                                       });
    }

    private static void MapSchemaConfig(IProfileExpression config)
    {
        config.CreateMap<UserDto, User>();
        config.CreateMap<UserAuthenticationDto, UserAuthentication>();

        config.CreateMap<InfoDto, Info>();
        config.CreateMap<ErrorDto, Error>();
    }

    private static void MapSchemaDtoConfig(IProfileExpression config)
    {
        config.CreateMap<User, UserDto>();
        config.CreateMap<UserAuthentication, UserAuthenticationDto>();
    }
    #endregion
}