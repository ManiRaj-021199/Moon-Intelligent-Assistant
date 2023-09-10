namespace MoonIntelligentAssistant.Logic;

internal static class ResponseBuilderHelper
{
    #region Internals
    internal static BaseApiResponseDto BuildSuccessResponse(string strResponseMessage, object result = null!)
    {
        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.OK,
                   ResponseMessage = strResponseMessage,
                   Result = result
               };
    }

    internal static BaseApiResponseDto BuildErrorResponse(object result = null!)
    {
        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.BadRequest,
                   ResponseMessage = CommonErrorMessages.SomethingWentWrong,
                   Result = result
               };
    }

    internal static BaseApiResponseDto BuildErrorResponse(string strResponseMessage, object result = null!)
    {
        return new BaseApiResponseDto
               {
                   ResponseCode = HttpStatusCode.BadRequest,
                   ResponseMessage = strResponseMessage,
                   Result = result
               };
    }
    #endregion
}