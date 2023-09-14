namespace MoonIntelligentAssistant.Gateways;

public static class ServiceGateways
{
    #region Publics
    public static void AddMainDbContext(this IServiceCollection services)
    {
        AddMoonDbContext(services);
    }

    public static void AddAuthenticationFacades(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationFacade, AuthenticationFacade>();
    }
    #endregion

    #region Privates
    private static void AddMoonDbContext(IServiceCollection services)
    {
        services.AddDbContext<MoonIaContext>();
    }
    #endregion
}