using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using PinPlatform.Common.Repositories;
using PinPlatform.Common.Verifiers;
using PinPlatform.Services.ClientApi.DataModel;

namespace PinPlatform.Services.ClientApi.Controllers
{
    [ApiController]
    public class PinChangeController : ControllerBase
    {
        private readonly ILogger<PinChangeController> _logger;
        private readonly IOpCoVerifier _opCoVerifier;
        private readonly IPinChangeVerifier _pinChangeVerifier;
        private readonly IPinRepository _pinRepository;
        private readonly IMapper _mapper;

        public PinChangeController(ILogger<PinChangeController> logger, IOpCoVerifier opCoVerifier, IPinChangeVerifier pinChangeVerifier, IPinRepository pinRepository, IMapper mapper)
        {
            _logger = logger;
            _opCoVerifier = opCoVerifier;
            _pinChangeVerifier = pinChangeVerifier;
            _pinRepository = pinRepository;
        }

        [HttpPost]
        [Route("v1/{opcoid}/pin")]
        [Route("v1/{opcoid}/pin/change")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorResponseModel))]
        public async Task<IActionResult> PostChangePinAsync([FromRoute] string opcoid, [FromBody] DataModel.PinChangeRequestModel request)
        {
            var requestor = _mapper.Map<RequestorInfo>(request.Requestor);
            requestor.OpCoId = opcoid;
            return await HandleChangeRequestAsync(requestor, request.PinType, request.NewPin);
        }

        [HttpPost]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pin/{pintype}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorResponseModel))]
        public async Task<IActionResult> PostChangePinAltAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromRoute] uint pintype, [FromBody] PinChangeModel pindetails)
        {
            var requestor = new RequestorInfo() { HouseholdId = householdid, ProfileId = profileid, OpCoId = opcoid };
            return await HandleChangeRequestAsync(requestor, pintype, pindetails.NewPin);
        }

        private async Task<IActionResult> HandleChangeRequestAsync(RequestorInfo requestor, uint pintype, string newpin)
        { 
            var opcoVerifyResult = _opCoVerifier.CheckIfOpCoHasPinService(requestor.OpCoId);
            if (!opcoVerifyResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)opcoVerifyResult.Error, ErrorText = ErrorTexts.GetTextForErrorCode(opcoVerifyResult.Error) });

            var pinChangeResult = _pinChangeVerifier.CheckNewPinAgainstRules(requestor.OpCoId, pintype, newpin);
            if (!pinChangeResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)pinChangeResult.Error, ErrorText = ErrorTexts.GetTextForErrorCode(pinChangeResult.Error) });

            await _pinRepository.SetPinAsync(requestor, pintype, newpin);

            return Ok();
        }
    }
}
