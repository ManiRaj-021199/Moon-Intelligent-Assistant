namespace MoonIntelligentAssistant.Containers;

public static class ServiceContainers
{
    #region Publics
    public static void AddMoonDbContext(this IServiceCollection services)
    {
        services.AddDbContext<MoonIaContext>();
    }

    public static void AddAuthenticationFacades(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationFacade, AuthenticationFacade>();
    }
    #endregion
}