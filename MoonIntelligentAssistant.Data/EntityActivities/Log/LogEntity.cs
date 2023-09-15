namespace MoonIntelligentAssistant.Data;

public class LogEntity : ILogEntity
{
    #region Fields
    private readonly MoonIaContext dbContext;
    #endregion

    #region Constructors
    public LogEntity(MoonIaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Publics
    public void AddLogInfo(InfoDto dtoInfo)
    {
        dbContext.Infos.Add(dtoInfo.ToInfo());
        dbContext.SaveChanges();
    }

    public void AddLogError(ErrorDto dtoError)
    {
        dbContext.Errors.Add(dtoError.ToError());
        dbContext.SaveChanges();
    }
    #endregion
}