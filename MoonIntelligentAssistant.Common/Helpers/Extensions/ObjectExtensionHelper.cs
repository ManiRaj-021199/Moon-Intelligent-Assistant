namespace MoonIntelligentAssistant.Common;

public static class ObjectExtensionHelper
{
    #region Publics
    public static string ToFlatString(this object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
    #endregion
}