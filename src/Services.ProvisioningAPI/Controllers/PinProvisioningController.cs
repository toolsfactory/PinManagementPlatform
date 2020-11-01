using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Processors;
using PinPlatform.Services.Infrastructure.Authorization;
using PinPlatform.Services.ProvisioningAPI.DataModel;

namespace PinPlatform.Services.ProvisioningAPI.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthorizationHelper.ProvisioningAccessPolicy)]

    public class PinProvisioningController : ControllerBase
    {
        private readonly ILogger<PinProvisioningController> _logger;
        private readonly ILockPinProcessor _lockPinProcessor;
        private readonly IDeletePinProcessor _deletePinProcessor;
        private readonly ICreatePinProcessor _createPinProcessor;

        public PinProvisioningController(ILogger<PinProvisioningController> logger, ILockPinProcessor lockPinProcessor, IDeletePinProcessor deletePinProcessor, ICreatePinProcessor createPinProcessor)
        {
            _logger = logger;
            _lockPinProcessor = lockPinProcessor;
            _deletePinProcessor = deletePinProcessor;
            _createPinProcessor = createPinProcessor;
        }

        [HttpPost]
        [Route("v1/{opcoid}/{profileid}/pin")]
        [Route("v1/{opcoid}/{profileid}/pin/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePinAsync([FromRoute] string opcoid, [FromBody] DataModel.PinCreateModel request)
        {
            var requestor = new Domain.Models.RequestorModel()
            {
                HouseholdId = request.HouseholdId,
                ProfileId = request.ProfileId,
                OpCoId = opcoid,
                PinType = request.PinType
            };
            await _createPinProcessor.ProcessRequestAsync(requestor, request.NewPin);
            return Ok();
        }

        [HttpDelete]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pin/{pintype}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePinAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromRoute] uint pintype)
        {
            var requestor = new Domain.Models.RequestorModel()
            {
                HouseholdId = householdid,
                ProfileId = profileid,
                OpCoId = opcoid,
                PinType = pintype
            };
            await _deletePinProcessor.ProcessRequestAsync(requestor);
            return Ok();
        }

        [HttpPost]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pin/{pintype}/lock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LockPinAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromRoute] uint pintype, [FromBody] DataModel.PinLockModel request)
        {
            var requestor = new Domain.Models.RequestorModel()
            {
                HouseholdId = householdid,
                ProfileId = profileid,
                OpCoId = opcoid,
                PinType = pintype
            };
            await _lockPinProcessor.ProcessRequestAsync(requestor, true, request.Reason);
            return Ok();
        }

        [HttpPost]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pin/{pintype}/unlock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UnlockPinAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid, [FromRoute] uint pintype, [FromBody] DataModel.PinLockModel request)
        {
            var requestor = new Domain.Models.RequestorModel()
            {
                HouseholdId = householdid,
                ProfileId = profileid,
                OpCoId = opcoid,
                PinType = pintype
            };
            await _lockPinProcessor.ProcessRequestAsync(requestor, false);
            return Ok();
        }
    }
}
