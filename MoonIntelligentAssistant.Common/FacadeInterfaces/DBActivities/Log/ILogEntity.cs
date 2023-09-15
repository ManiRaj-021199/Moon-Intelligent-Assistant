namespace MoonIntelligentAssistant.Common;

public interface ILogEntity
{
    void AddLogInfo(InfoDto dtoInfo);
    void AddLogError(ErrorDto dtoError);
}