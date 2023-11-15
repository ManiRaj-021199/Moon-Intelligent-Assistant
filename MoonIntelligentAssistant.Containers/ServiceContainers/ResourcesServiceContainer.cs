namespace MoonIntelligentAssistant.Containers;

internal static class ResourcesServiceContainer
{
    #region Internals
    internal static void AddResourcesFacades(this IServiceCollection services)
    {
        // TODO: Need to add facades
    }

    internal static void AddResourcesEntities(this IServiceCollection services)
    {
        // Common DbContext Activities
        services.AddScoped<ICommonDBContextActivities, CommonDBContextActivities>();
        services.AddScoped<ILogEntity, LogEntity>();

        // Entities
        // TODO: Need to add entities
    }
    #endregion
}