using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace PinPlatform.Services.Infrastructure.Authentication
{
    public static class AuthenticationConfigurationExtension
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, 
                                                           IConfiguration config)
        {
            var securityKeyProvider = services.BuildServiceProvider().GetRequiredService<ISecurityKeyProvider>();
            var tokenHeader = config.GetValue<string>(AuthConstants.ConfigKeyHeaderName, AuthConstants.DefaultTokenHeader);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = config.GetValue(AuthConstants.ConfigKeyValidIssuer, AuthConstants.DefaultValidIssuer),
                    ValidAudience = config.GetValue(AuthConstants.ConfigKeyValidAudience, AuthConstants.DefaultValidAudience),
                    ClockSkew = TimeSpan.FromSeconds(5),
                    IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                    {
                        var key = securityKeyProvider.Keys.Where(k => k.Key == kid).FirstOrDefault().Value;
                        List<SecurityKey> keys = new List<SecurityKey>();
                        if (key != null)
                        {
                            keys.Add(key);
                        }
                        return keys;
                    }
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        string authToken = context.Request.Headers[tokenHeader];

                        // If no authorization header found, nothing to process further
                        if (string.IsNullOrWhiteSpace(authToken))
                        {
                            context.NoResult();
                            return Task.CompletedTask;
                        }

                         context.Token = authToken.Trim();

                        // If no token found, no further work possible
                        if (string.IsNullOrEmpty(context.Token))
                        {
                            context.NoResult();
                            return Task.CompletedTask;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
    }
}
