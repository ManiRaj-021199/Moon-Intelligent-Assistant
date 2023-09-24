namespace MoonIntelligentAssistant.Containers;

internal static class AuthenticationServiceContainer
{
    #region Internals
    internal static void AddAuthenticationFacades(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationFacade, AuthenticationFacade>();
    }

    internal static void AddAuthenticationEntities(this IServiceCollection services)
    {
        // Common DbContext Activities
        services.AddScoped<ICommonDBContextActivities, CommonDBContextActivities>();
        services.AddScoped<ILogEntity, LogEntity>();

        // Entities
        services.AddScoped<IUsersEntity, UsersEntity>();
        services.AddScoped<IUserAuthenticationEntity, UserAuthenticationEntity>();
    }
    #endregion
}