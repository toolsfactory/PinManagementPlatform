using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using PinPlatform.Common.Verifiers;
using PinPlatform.Services.ClientApi.DataModel;

namespace PinPlatform.Services.ClientApi.Controllers
{
    [ApiController]
    public class PinVerifyController : ControllerBase
    {
        private readonly ILogger<PinVerifyController> _logger;
        private readonly IPinHashVerifier _pinCheckVerifier;
        private readonly IOpCoVerifier _opCoVerifier;
        private readonly IMapper _mapper;

        public PinVerifyController(ILogger<PinVerifyController> logger, IPinHashVerifier pinCheckVerifier, IOpCoVerifier opCoVerifier, IMapper mapper)
        {
            _logger = logger;
            _pinCheckVerifier = pinCheckVerifier;
            _opCoVerifier = opCoVerifier;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("v1/{opcoid}/pin/verify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [FormatFilter]
        public async Task<ActionResult<ApiResponse>> GetVerifyPinAsync([FromRoute] string opcoid, [FromBody] DataModel.PinVerifyRequestModel request)
        {
            var requestor = _mapper.Map<RequestorInfo>(request.Requestor);
            requestor.OpCoId = opcoid;
            return await HandleVerifyRequestAsync(requestor, request.PinType, request.PinHash);
        }

        [HttpGet]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pins/{pintype}/verify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> GetVerifyPinAlternativeAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromRoute] uint pintype, [FromQuery] string pinhash)
        {
            var requestor = new RequestorInfo() { HouseholdId = householdid, ProfileId = profileid, OpCoId = opcoid };
            return await HandleVerifyRequestAsync(requestor, pintype, pinhash);
        }

        [HttpPost]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pin/verify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> PostVerifyPinAlternativeAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromBody] PinHashModel pinDetails)
        {
            var requestor = new RequestorInfo() { HouseholdId = householdid, ProfileId = profileid, OpCoId = opcoid };
            return await HandleVerifyRequestAsync(requestor, pinDetails.PinType, pinDetails.PinHash);
        }


        private async Task<ActionResult<ApiResponse>> HandleVerifyRequestAsync(RequestorInfo requestor, uint pintype, string hash)
        {
            var opcoVerifyResult = _opCoVerifier.CheckIfOpCoHasPinService(requestor.OpCoId);
            if (!opcoVerifyResult.Success)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, opcoVerifyResult.Error.ToString(), this.HttpContext.TraceIdentifier, ErrorTexts.GetTextForErrorCode(opcoVerifyResult.Error)));

            var pinVerifyResult = await _pinCheckVerifier.VerifyPinHashAsync(requestor, pintype, hash);
            if (!pinVerifyResult.Success)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, opcoVerifyResult.Error.ToString(), this.HttpContext.TraceIdentifier, ErrorTexts.GetTextForErrorCode(opcoVerifyResult.Error)));

            return Ok();
        }
    }
}
