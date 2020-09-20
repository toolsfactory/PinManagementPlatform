using Microsoft.AspNetCore.Mvc;
using PinPlatform.Common.DataModels;
using PinPlatform.Common;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerBaseExtensions
    {
        public static BadRequestObjectResult BadRequestWithErrorCode(this ControllerBase cb, ErrorCodes error)
        {
            return cb.BadRequest(new ErrorResponseModel() { ErrorCode = (int)error, ErrorText = ErrorTexts.GetTextForErrorCode(error) });
        }

        public static BadRequestObjectResult BadRequestWithErrorCode(this ControllerBase cb, ErrorCodes error, object data)
        {
            return cb.BadRequest(new ExtendedErrorResponseModel() { ErrorCode = (int)error, ErrorText = ErrorTexts.GetTextForErrorCode(error), Data = data });
        }
    }
}
