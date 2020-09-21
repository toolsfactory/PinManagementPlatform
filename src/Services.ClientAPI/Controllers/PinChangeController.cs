using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using PinPlatform.Common.Repositories;
using PinPlatform.Common.Verifiers;

namespace PinPlatform.Services.ClientApi.Controllers
{
    [ApiController]
    public class PinChangeController: ControllerBase
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
        [Route("v1/{opcoid}/pin/change")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorResponseModel))]
        public async Task<IActionResult> ChangePinAsync([FromRoute] string opcoid, [FromBody]DataModel.PinChangeRequestModel request)
        {
            var opcoVerifyResult = _opCoVerifier.CheckIfOpCoHasPinService(opcoid);
            if (!opcoVerifyResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)opcoVerifyResult.Error, ErrorText = ErrorTexts.GetTextForErrorCode(opcoVerifyResult.Error) });

            var pinChangeResult = _pinChangeVerifier.CheckNewPinAgainstRules(opcoid, request.PinType, request.NewPin);
            if (!pinChangeResult.Success)
                return BadRequest(new ErrorResponseModel() { ErrorCode = (int)pinChangeResult.Error, ErrorText = ErrorTexts.GetTextForErrorCode(pinChangeResult.Error) });

            var requestor = _mapper.Map<RequestorInfo>(request.Requestor);
            requestor.OpCoId = opcoid;
            await _pinRepository.SetPinAsync(requestor, request.PinType, request.NewPin);

            return Ok();
        }
    }
}
