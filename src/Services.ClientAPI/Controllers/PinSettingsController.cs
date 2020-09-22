using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using PinPlatform.Common.Verifiers;
using PinPlatform.Services.ClientApi.DataModel;

namespace PinPlatform.Services.ClientApi.Controllers
{
    [ApiController]
    public class PinSettingsController : ControllerBase
    {
        private readonly ILogger<PinVerifyController> _logger;
        private readonly IPinHashVerifier _pinCheckVerifier;
        private readonly IOpCoVerifier _opCoVerifier;
        private readonly IMapper _mapper;

        public PinSettingsController(ILogger<PinVerifyController> logger, IPinHashVerifier pinCheckVerifier, IOpCoVerifier opCoVerifier, IMapper mapper)
        {
            _logger = logger;
            _pinCheckVerifier = pinCheckVerifier;
            _opCoVerifier = opCoVerifier;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("v1/{opcoid}/pin/settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [FormatFilter]
        public async Task<ActionResult<ApiResponse>> GetPinSettingsAsync([FromRoute] string opcoid, [FromBody] DataModel.RequestorModel requestor)
        {
            var req = _mapper.Map<RequestorInfo>(requestor);
            req.OpCoId = opcoid;
            return await HandleSettingsRequestAsync(req);
        }

        [HttpGet]
        [Route("v1/{opcoid}/{householdid}/{profileid}/pins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> GetPinSettingsAlternativeAsync([FromRoute] string opcoid, [FromRoute] string householdid, [FromRoute] uint profileid)
        {
            var req = new RequestorInfo() { OpCoId = opcoid, HouseholdId = householdid, ProfileId = profileid };
            return await HandleSettingsRequestAsync(req);
        }


        private async Task<ActionResult<ApiResponse>> HandleSettingsRequestAsync(RequestorInfo req)
        {
            return Ok("settings");
        }
    }
}
