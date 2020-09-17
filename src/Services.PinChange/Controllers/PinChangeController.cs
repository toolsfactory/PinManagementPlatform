using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using PinPlatform.Common.Interfaces;

namespace PinPlatform.Services.PinChange.Controllers
{
    [ApiController]
    [Route("v1/{opcoid}/[controller]")]
    public class PinChangeController: ControllerBase
    {
        private readonly ILogger<PinChangeController> _logger;
        private readonly IOpCoVerifier _opCoVerifier;
        private readonly IPinChangeVerifier _pinChangeVerifier;

        public PinChangeController(ILogger<PinChangeController> logger, IOpCoVerifier opCoVerifier, IPinChangeVerifier pinChangeVerifier)
        {
            _logger = logger;
            _opCoVerifier = opCoVerifier;
            _pinChangeVerifier = pinChangeVerifier;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorResponseModel))]
        public async Task<IActionResult> PostAsync([FromRoute] string opcoid, [FromBody]DataModel.PinChangeRequestModel request)
        {
            var opcoVerifyResult = _opCoVerifier.CheckIfOpCoHasPinService(opcoid);
            if (!opcoVerifyResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)opcoVerifyResult.Error, ErrorText = ErrorTexts.GetTextForErrorCode(opcoVerifyResult.Error) });

            var pinChangeResult = _pinChangeVerifier.CheckNewPinAgainstRules(opcoid, request.PinType, request.NewPin);
            if (!pinChangeResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)pinChangeResult.Error, ErrorText = ErrorTexts.GetTextForErrorCode(pinChangeResult.Error) });
            
            return Ok();
        }
    }
}
