namespace MoonIntelligentAssistant.Containers;

public static class ServiceContainers
{
    #region Publics
    public static void AddMainDbContext(this IServiceCollection services)
    {
        AddMoonDbContext(services);
        AddMoonDbEntities(services);
    }

    public static void AddAuthenticationFacades(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationFacade, AuthenticationFacade>();
    }
    #endregion

    #region Privates
    private static void AddMoonDbContext(IServiceCollection services)
    {
        // Main Database
        services.AddDbContext<MoonIaContext>();
    }

    private static void AddMoonDbEntities(IServiceCollection services)
    {
        // Common DbContext Activities
        services.AddScoped<ICommonDBContextActivities, CommonDBContextActivities>();

        // Entities
        services.AddScoped<IAuthUsersEntity, AuthUsersEntity>();
        services.AddScoped<INonAuthUsersEntity, NonAuthUsersEntity>();
    }
    #endregion
}