using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Processors;

namespace PinPlatform.Services.ClientAPI.Controllers
{
    [ApiController]
    public class PinSettingsController : ControllerBase
    {
        private readonly ILogger<PinVerifyController> _logger;
        private readonly IPinSettingsProcessor _processor;

        public PinSettingsController(ILogger<PinVerifyController> logger, IPinSettingsProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        [HttpPost]
        [Route("v1/{opcoid}/pin/settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPinSettingsAsync([FromRoute] string opcoid, [FromBody] DataModel.RequestorModel requestor)
        {
            var req = new Domain.Models.RequestorModel()
            {
                OpCoId = opcoid,
                HouseholdId = requestor.HouseholdId,
                ProfileId = requestor.ProfileId
            };
            var result = await _processor.ProcessRequestAsync(req);
            return Ok(result);
        }

        [HttpGet]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPinSettingsAlternativeAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid)
        {
            var req = new Domain.Models.RequestorModel()
            {
                OpCoId = opcoid,
                HouseholdId = householdid,
                ProfileId = profileid
            };
            var result = await _processor.ProcessRequestAsync(req);
            return Ok(result);
        }
    }
}
