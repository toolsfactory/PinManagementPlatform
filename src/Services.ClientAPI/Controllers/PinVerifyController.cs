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
    [ApiController]
    [Authorize(Policy = AuthorizationHelper.ClientAccessPolicy)]
    [Route("v{version:apiversion}/{opcoid}")]
    public class PinVerifyController : ControllerBase
    {
        private readonly ILogger<PinVerifyController> _logger;
        private readonly IVerifyPinProcessor _processor;

        public PinVerifyController(ILogger<PinVerifyController> logger, IVerifyPinProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        [HttpPost]
        [Route("pin/verify")]
        [ApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetVerifyPinAsync([FromRoute] string opcoid, [FromBody] DataModel.PinVerifyRequestModel request)
        {
            var data = new Domain.Processors.VerifyPinParameters()
            {
                Requestor = new Domain.Models.RequestorModel() 
                { 
                    HouseholdId = request.Requestor.HouseholdId, 
                    ProfileId = request.Requestor.ProfileId, 
                    OpCoId = opcoid,
                    PinType = request.PinType
                },
                PinHash = request.PinHash
            };
            await _processor.ProcessRequestAsync(data);
            return Ok();
        }

        [HttpGet]
        [Route("{householdid}/{profileid}/pins/{pintype}/verify")]
        [ApiVersion("1.0")]
        [ApiVersion("1.1")]
        [ApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetVerifyPinAlternativeAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromRoute] uint pintype, [FromQuery] string pinhash)
        {
            var data = new Domain.Processors.VerifyPinParameters()
            {
                Requestor = new Domain.Models.RequestorModel() 
                { 
                    HouseholdId = householdid, 
                    ProfileId = profileid, 
                    OpCoId = opcoid,
                    PinType = pintype
                },
                PinHash = pinhash
            };
            await _processor.ProcessRequestAsync(data);
            return Ok();
        }

        [HttpPost]
        [Route("{householdid}/{profileid}/pin/verify")]
        [ApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PostVerifyPinAlternativeAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromBody] PinHashModel pinDetails)
        {
            var data = new Domain.Processors.VerifyPinParameters()
            {
                Requestor = new Domain.Models.RequestorModel() 
                { 
                    HouseholdId = householdid, 
                    ProfileId = profileid, 
                    OpCoId = opcoid,
                    PinType = pinDetails.PinType
                },
                PinHash = pinDetails.PinHash
            };
            await _processor.ProcessRequestAsync(data);
            return Ok();
        }
    }
}
