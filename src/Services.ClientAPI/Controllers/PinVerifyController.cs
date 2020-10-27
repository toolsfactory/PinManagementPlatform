using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Processors;
using PinPlatform.Services.ClientAPI.DataModel;

namespace PinPlatform.Services.ClientAPI.Controllers
{
    [ApiController]
    [Authorize(Policy = "ClientAccess")]
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
        [Route("v1/{opcoid}/pin/verify")]
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
        [Route("v1/{opcoid}/{householdid}/{profileid}/pins/{pintype}/verify")]
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
        [Route("v1/{opcoid}/{householdid}/{profileid}/pin/verify")]
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
