namespace MoonIntelligentAssistant.Logic;

internal static class ResponseBuilderHelper
{
    #region Fields
    internal static object objApiRequest = string.Empty;
    internal static ILogEntity entityLog = null!;
    #endregion

    #region Internals
    internal static BaseApiResponseDto BuildSuccessResponse(string strResponseMessage, [CallerFilePath] string strCallerFilePath = "", [CallerMemberName] string strCallerMemberName = "")
    {
        BaseApiResponseDto dtoResponse = new()
                                         {
                                             ResponseCode = HttpStatusCode.OK,
                                             ResponseMessage = strResponseMessage
                                         };
        entityLog.AddLogInfo(GetInfoDto(GetApiName(strCallerFilePath, strCallerMemberName), dtoResponse, ApiSeverity.INFORMATION));

        return dtoResponse;
    }

    internal static BaseApiResponseDto BuildSuccessResponse(string strResponseMessage, object result, [CallerFilePath] string strCallerFilePath = "", [CallerMemberName] string strCallerMemberName = "")
    {
        BaseApiResponseDto dtoResponse = new()
                                         {
                                             ResponseCode = HttpStatusCode.OK,
                                             ResponseMessage = strResponseMessage,
                                             Result = result
                                         };
        entityLog.AddLogInfo(GetInfoDto(GetApiName(strCallerFilePath, strCallerMemberName), dtoResponse, ApiSeverity.INFORMATION));

        return dtoResponse;
    }

    internal static BaseApiResponseDto BuildWarningResponse(string strResponseMessage, [CallerFilePath] string strCallerFilePath = "", [CallerMemberName] string strCallerMemberName = "")
    {
        BaseApiResponseDto dtoResponse = new()
                                         {
                                             ResponseCode = HttpStatusCode.Accepted,
                                             ResponseMessage = strResponseMessage
                                         };
        entityLog.AddLogInfo(GetInfoDto(GetApiName(strCallerFilePath, strCallerMemberName), dtoResponse, ApiSeverity.WARNING));

        return dtoResponse;
    }

    internal static BaseApiResponseDto BuildErrorResponse(Exception exception, [CallerFilePath] string strCallerFilePath = "", [CallerMemberName] string strCallerMemberName = "")
    {
        entityLog.AddLogError(GetErrorDto(GetApiName(strCallerFilePath, strCallerMemberName), exception));

        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.BadRequest,
                   ResponseMessage = CommonErrorMessages.SomethingWentWrong
               };
    }
    #endregion

    #region Privates
    private static string GetApiName(string strCallerFilePath, string strCallerMemberName)
    {
        string strClassNameWithoutExtension = Path.GetFileNameWithoutExtension(strCallerFilePath);

        return $"{strClassNameWithoutExtension} - {strCallerMemberName}";
    }

    private static InfoDto GetInfoDto(string strApiName, object objResponse, ApiSeverity severity)
    {
        return new InfoDto
               {
                   ApiName = strApiName,
                   ApiRequest = objApiRequest.ToFlatString(),
                   ApiResponse = objResponse.ToFlatString(),
                   ApiSeverity = severity.ToString(),
                   LogDate = DateTimeUtilitiesHelper.GetCurrentDateTime()
               };
    }

    private static ErrorDto GetErrorDto(string strApiName, object objException)
    {
        return new ErrorDto
               {
                   ApiName = strApiName,
                   ApiRequest = objApiRequest.ToFlatString(),
                   Exception = objException.ToFlatString(),
                   LogDate = DateTimeUtilitiesHelper.GetCurrentDateTime()
               };
    }
    #endregion
}