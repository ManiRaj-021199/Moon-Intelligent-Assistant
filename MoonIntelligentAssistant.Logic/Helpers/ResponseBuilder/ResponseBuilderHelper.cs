namespace MoonIntelligentAssistant.Logic;

internal static class ResponseBuilderHelper
{
    #region Internals
    internal static BaseApiResponseDto BuildSuccessResponse(string strResponseMessage)
    {
        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.OK,
                   ResponseMessage = strResponseMessage
               };
    }

    internal static BaseApiResponseDto BuildSuccessResponse(string strResponseMessage, object result)
    {
        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.OK,
                   ResponseMessage = strResponseMessage,
                   Result = result
               };
    }

    internal static BaseApiResponseDto BuildWarningResponse(string strResponseMessage)
    {
        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.Accepted,
                   ResponseMessage = strResponseMessage
               };
    }

    internal static BaseApiResponseDto BuildErrorResponse(Exception exception)
    {
        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.BadRequest,
                   ResponseMessage = CommonErrorMessages.SomethingWentWrong
               };
    }
    #endregion
}