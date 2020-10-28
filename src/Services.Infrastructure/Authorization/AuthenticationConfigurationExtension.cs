using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PinPlatform.Services.Infrastructure.Authorization
{
    public static class AuthenticationConfigurationExtension
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy("ClientAccess", policy => policy.RequireClaim("x-client-access", "true"));
                x.AddPolicy("AdminAccess", policy => policy.RequireClaim("x-admin-access", "true"));
                x.AddPolicy("ProvisioningAccess", policy => policy.RequireClaim("x-provisioning-access", "true"));
            });
            return services;
        }
    }
}
