﻿namespace MoonIntelligentAssistant.Data;

internal static class MoonDBContextBase
{
    #region Internals
    internal static void ClearChangeTracker(this MoonIaContext dbContext)
    {
        dbContext.ChangeTracker.Clear();
        dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    #endregion
}