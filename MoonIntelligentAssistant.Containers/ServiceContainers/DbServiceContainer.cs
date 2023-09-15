namespace MoonIntelligentAssistant.Containers;

public static class DbServiceContainer
{
    #region Publics
    public static void AddMainDbContext(this IServiceCollection services)
    {
        AddMoonDbContext(services);
    }

    public static void AddAuthenticationContainer(this IServiceCollection services)
    {
        services.AddAuthenticationFacades();
        services.AddAuthenticationEntities();
    }
    #endregion

    #region Privates
    private static void AddMoonDbContext(IServiceCollection services)
    {
        // Main Database
        services.AddDbContext<MoonIaContext>();
    }
    #endregion
}