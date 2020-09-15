﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common.DataModels;
using PinPlatform.Common.Interfaces;

namespace PinPlatform.Services.PinVerify.Controllers
{
    [ApiController]
    [Route("v1/{opcoid}/[controller]")]
    public class PinVerifyController : ControllerBase
    {
        private readonly ILogger<PinVerifyController> _logger;
        private readonly IPinHashVerifier _pinCheckVerifier;
        private readonly IOpCoVerifier _opCoVerifier;

        public PinVerifyController(ILogger<PinVerifyController> logger, IPinHashVerifier pinCheckVerifier, IOpCoVerifier opCoVerifier)
        {
            _logger = logger;
            _pinCheckVerifier = pinCheckVerifier;
            _opCoVerifier = opCoVerifier;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorResponseModel))]
        public async Task<IActionResult> PostAsync([FromRoute] string opcoid, [FromBody] DataModel.PinVerifyRequestModel request)
        {
            var opcoVerifyResult = _opCoVerifier.CheckIfOpCoHasPinService(opcoid);
            if (!opcoVerifyResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)opcoVerifyResult.Error, ErrorText = "Validation error" });

            var pinVerifyResult = await _pinCheckVerifier.VerifyPinHashAsync(request.Requestor!, request.PinType, request.PinHash);
            if (!pinVerifyResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)pinVerifyResult.Error, ErrorText = "Validation error" });

            return Ok();
        }

    }
}
