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
using PinPlatform.Domain.Processors;
using PinPlatform.Services.ClientApi.DataModel;

namespace PinPlatform.Services.ClientApi.Controllers
{
    [ApiController]
    public class PinVerifyController : ControllerBase
    {
        private readonly ILogger<PinVerifyController> _logger;
        private readonly IVerifyPinProcessor _processor;
        private readonly IMapper _mapper;

        public PinVerifyController(ILogger<PinVerifyController> logger, IVerifyPinProcessor processor, IMapper mapper)
        {
            _logger = logger;
            _processor = processor;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("v1/{opcoid}/pin/verify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [FormatFilter]
        public async Task<ActionResult<ApiResponse>> GetVerifyPinAsync([FromRoute] string opcoid, [FromBody] DataModel.PinVerifyRequestModel request)
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
