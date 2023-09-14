namespace MoonIntelligentAssistant.Data;

internal static class MoonDBContextBase
{
    #region Internals
    internal static void TurnOffQueryTrackingBehaviour(this MoonIaContext dbContext)
    {
        dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    #endregion
}