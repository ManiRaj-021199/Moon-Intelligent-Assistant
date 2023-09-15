namespace MoonIntelligentAssistant.Data;

internal static class LogDtoAutoMapperHelper
{
    #region Internals
    internal static Info ToInfo(this InfoDto dtoInfo)
    {
        return AutoMapperInitializer.Mapper.Map<Info>(dtoInfo);
    }

    internal static Error ToError(this ErrorDto dtoError)
    {
        return AutoMapperInitializer.Mapper.Map<Error>(dtoError);
    }
    #endregion
}