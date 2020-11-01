using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Processors;
using PinPlatform.Services.Infrastructure.Authorization;

namespace PinPlatform.Services.ClientAPI.Controllers
{
    [ApiController]
    [Route("v{version:apiversion}/{opcoid}")]
    [Authorize(Policy = AuthorizationHelper.ClientAccessPolicy)]
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
        [Route("pin/settings")]
        [ApiVersion("1")]
        [ApiVersion("1.1")]
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
        [Route("{householdid}/{profileid}/pins")]
        [ApiVersion("1.0")]
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
