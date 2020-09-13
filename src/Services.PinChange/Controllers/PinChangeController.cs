using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using PinPlatform.Common.DataModels.Caching;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace PinPlatform.Services.PinChange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PinChangeController: ControllerBase
    {
        private readonly ILogger<PinChangeController> _logger;
        private readonly PinHashVerifier _verifier;

        public PinChangeController(ILogger<PinChangeController> logger, PinHashVerifier verifier)
        {
            _logger = logger;
            _verifier = verifier;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorResponseModel))]
        public async Task<IActionResult> PostAsync([FromBody]DataModel.PinChangeRequestModel request)
        {
            var (success, error) = await _verifier.VerifyPinHashAsync(request.Requestor, request.PinType, request.OldPinHash);

            if (!success)
                return BadRequest(new ErrorResponseModel() { ErrorCode =(int) error, ErrorText = "Validation error" });

            return Ok();
        }

    }
}
