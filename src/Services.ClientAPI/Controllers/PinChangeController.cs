using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Processors;
using PinPlatform.Services.ClientAPI.DataModel;
using PinPlatform.Services.Infrastructure.Authorization;

namespace PinPlatform.Services.ClientAPI.Controllers
{
    /// <summary>
    /// Everything around changing the pin from customer side
    /// </summary>
    [ApiController]
    [Route("v{version:apiversion}/{opcoid}")]
    [Authorize(Policy = AuthorizationHelper.ClientAccessPolicy)]
    public class PinChangeController : ControllerBase
    {
        private readonly ILogger<PinChangeController> _logger;
        private readonly IChangePinProcessor _pinChangeProcessor;

        public PinChangeController(ILogger<PinChangeController> logger,IChangePinProcessor pinChangeProcessor)
        {
            _logger = logger;
            _pinChangeProcessor = pinChangeProcessor;
        }
        /// <summary>
        /// Change Pin with most of the information transfered in the http body
        /// </summary>
        /// <param name="opcoid">The OpCo ID</param>
        /// <param name="request">The request details in the http body formatted as JSON</param>
        /// <returns></returns>
        [HttpPost]
        [Route("pin")]
        [Route("pin/change")]
        [ApiVersion("1.0")]
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

        /// <summary>
        /// Change Pin with all information except the pins transfered in the URL
        /// </summary>
        /// <param name="opcoid">The OpCo ID</param>
        /// <param name="householdid">The household ID</param>
        /// <param name="profileid">The provide ID</param>
        /// <param name="pintype">The pintype as number</param>
        /// <param name="pindetails">The hash of the old pin and the new pin as JSON formatted body</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{householdid}/{profileid}/pin/{pintype}")]
        [ApiVersion("1.0")]
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
