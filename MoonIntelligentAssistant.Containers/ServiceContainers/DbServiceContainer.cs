namespace MoonIntelligentAssistant.Containers;

public static class DbServiceContainer
{
    #region Publics
    public static void AddMainDbContext(this IServiceCollection services, string strDbConnection)
    {
        AddMoonDbContext(services, strDbConnection);
    }

    public static void AddAuthenticationContainer(this IServiceCollection services)
    {
        services.AddAuthenticationFacades();
        services.AddAuthenticationEntities();
    }

    public static void AddResourcesContainer(this IServiceCollection services)
    {
        services.AddResourcesFacades();
        services.AddResourcesEntities();
    }
    #endregion

    #region Privates
    private static void AddMoonDbContext(IServiceCollection services, string strDbConnection)
    {
        // Main Database
        services.AddDbContext<MoonIaContext>(options => options.UseSqlServer(strDbConnection));
    }
    #endregion
}