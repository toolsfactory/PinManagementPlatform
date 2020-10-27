using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Processors;
using PinPlatform.Services.ClientAPI.DataModel;

namespace PinPlatform.Services.ClientAPI.Controllers
{
    [ApiController]
    public class PinChangeController : ControllerBase
    {
        private readonly ILogger<PinChangeController> _logger;
        private readonly IChangePinProcessor _pinChangeProcessor;

        public PinChangeController(ILogger<PinChangeController> logger,IChangePinProcessor pinChangeProcessor)
        {
            _logger = logger;
            _pinChangeProcessor = pinChangeProcessor;
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
