
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using PinPlatform.Services.Infrastructure.Authentication;
using System.Linq;

namespace PinPlatform.Services.AdminAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public TokenController(ILogger<TokenController> logger, IConfiguration configuration, IJwtTokenGenerator tokenGenerator)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        [HttpGet]
        [Route("Issue")]
        public IActionResult GetToken([FromQuery] string keyId, [FromQuery]string tokenType = "client", [FromQuery] ushort expiresSec = 90)
        {
            tokenType ??= "client";
            var claims = new List<Claim>();
            if (tokenType.ToLower() == "admin")
                claims.Add(new Claim("x-admin-access", "true"));
            else
                claims.Add(new Claim("x-client-access", "true"));

            try
            {
                var token = _tokenGenerator.GenerateToken(claims, keyId, expiresSec);
                return Ok(new { jwt = token.Token, unixTimeExpiresAt = token.ExpiresAt, expiresAt = UnixTimeStampToDateTime(token.ExpiresAt).ToString() });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { Error = "KeyId not existing" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "Unknown exception", Exception = ex });
            }
        }

        [HttpGet]
        [Authorize] // Uses the default configured scheme, in this case "Bearer".
        [Route("Validate")]
        public IActionResult ValidateAnyToken()
        {
            return InternalValidate();
        }

        [HttpGet]
        [Authorize(Policy ="AdminAccess")] // Uses the default configured scheme, in this case "Bearer".
        [Route("ValidateAdmin")]
        public IActionResult ValidateAdminToken()
        {
            return InternalValidate();
        }

        [HttpGet]
        [Authorize(Policy = "ClientAccess")] // Uses the default configured scheme, in this case "Bearer".
        [Route("ValidateClient")]
        public IActionResult ValidateClientToken()
        {
            return InternalValidate();
        }

        private IActionResult InternalValidate()
        {
            var item = this.User.Claims.Where(x => x.Type == "exp").FirstOrDefault();
            if (item != null)
            {
                if (long.TryParse(item.Value, out var value))
                    return Ok(new { expires = value, text = UnixTimeStampToDateTime(value).ToString() });
            }
            return Ok();
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStampMilli)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStampMilli).ToLocalTime();
            return dtDateTime;
        }

        [HttpGet]
        [Route("GenerateKeyPair")]
        public IActionResult GenerateKeyPair()
        {
            using RSA rsa = RSA.Create();
            var result = new StringBuilder();
            result.AppendLine("-----Private key-----");
            result.AppendLine(Convert.ToBase64String(rsa.ExportRSAPrivateKey()));
            result.AppendLine("-----Public key-----");
            result.AppendLine(Convert.ToBase64String(rsa.ExportRSAPublicKey()));
            return Ok(result.ToString());
        }
    }
}
