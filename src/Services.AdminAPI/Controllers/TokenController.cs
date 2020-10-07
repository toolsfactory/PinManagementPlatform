
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

namespace PinPlatform.Services.Administration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _configuration;

        public TokenController(ILogger<TokenController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Issue")]
        public IActionResult GetToken([FromQuery]string tokenType = "client")
        {
            var signingCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Symmetric:Key"])),
                algorithm: SecurityAlgorithms.HmacSha256);

            DateTime jwtDate = DateTime.Now;

            tokenType ??= "client";
            var claims = new List<Claim>();
            if (tokenType.ToLower() == "admin")
                claims.Add(new Claim("x-admin-access", "true"));
            else
                claims.Add(new Claim("x-client-access", "true"));

            var jwt = new JwtSecurityToken(
                audience: "jwt-test", // must match the audience in AddJwtBearer()
                issuer: "jwt-test", // must match the issuer in AddJwtBearer()

                // Add whatever claims you'd want the generated token to include
                claims: claims,
                notBefore: jwtDate,
                expires: jwtDate.AddSeconds(90), // Should be short lived. For logins, it's may be fine to use 24h

                // Provide a cryptographic key used to sign the token.
                // When dealing with symmetric keys then this must be
                // the same key used to validate the token.
                signingCredentials: signingCredentials
            );

            // Generate the actual token as a string
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Return some agreed upon or documented structure.
            return Ok(new
            {
                jwt = token,
                // Even if the expiration time is already a part of the token, it's common to be 
                // part of the response body.
                unixTimeExpiresAt = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds()
            });
        }

        [HttpGet]
        [Authorize] // Uses the default configured scheme, in this case "Bearer".
        [Route("Validate")]
        public IActionResult ValidateAnyToken()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize(Policy ="AdminAccess")] // Uses the default configured scheme, in this case "Bearer".
        [Route("ValidateAdmin")]
        public IActionResult ValidateAdminToken()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize(Policy = "ClientAccess")] // Uses the default configured scheme, in this case "Bearer".
        [Route("ValidateClient")]
        public IActionResult ValidateClientToken()
        {
            return Ok();
        }
    }
}
