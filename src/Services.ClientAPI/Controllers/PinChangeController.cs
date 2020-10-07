using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using PinPlatform.Domain.Processors;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Verifiers;
using PinPlatform.Services.ClientApi.DataModel;

namespace PinPlatform.Services.ClientApi.Controllers
{
    [ApiController]
    public class PinChangeController : ControllerBase
    {
        private readonly ILogger<PinChangeController> _logger;
        private readonly IChangePinProcessor _pinChangeProcessor;
        private readonly IMapper _mapper;

        public PinChangeController(ILogger<PinChangeController> logger,IChangePinProcessor pinChangeProcessor, IMapper mapper)
        {
            _logger = logger;
            this._pinChangeProcessor = pinChangeProcessor;
        }

        [HttpPost]
        [Route("v1/{opcoid}/pin")]
        [Route("v1/{opcoid}/pin/change")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostChangePinAsync([FromRoute] string opcoid, [FromBody] DataModel.PinChangeRequestModel request)
        {
            var data = new ChangePinParameters()
            {
                Requestor = new Domain.Models.RequestorModel() { HouseholdId = request.Requestor.HouseholdId, ProfileId = request.Requestor.ProfileId, OpCoId = opcoid },
                PinType = request.PinType,
                OldPinHash = request.OldPinHash,
                NewPin = request.NewPin
            };
            await _pinChangeProcessor.ProcessRequestAsync(data);
            return Ok();
        }

        [HttpPost]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pin/{pintype}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostChangePinAltAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromRoute] uint pintype, [FromBody] PinChangeModel pindetails)
        {
            var data = new ChangePinParameters()
            {
                Requestor = new Domain.Models.RequestorModel() { HouseholdId = householdid, ProfileId = profileid, OpCoId = opcoid },
                PinType = pintype,
                OldPinHash = pindetails.OldPinHash,
                NewPin = pindetails.NewPin
            };
            await _pinChangeProcessor.ProcessRequestAsync(data);
            return Ok();
        }
    }
}
