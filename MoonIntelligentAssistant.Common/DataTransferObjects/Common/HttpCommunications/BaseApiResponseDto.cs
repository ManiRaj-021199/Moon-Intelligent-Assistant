namespace MoonIntelligentAssistant.Common;

public class BaseApiResponseDto
{
    #region Properties
    public HttpStatusCode ResponseCode { get; set; }
    public string ResponseMessage { get; set; } = string.Empty;
    public object? Result { get; set; }
    #endregion
}