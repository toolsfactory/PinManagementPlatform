using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PinPlatform.Services.Infrastructure.Authorization
{
    public static class AuthorizationConfigurationExtension
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration config)
            => services.AddAuthorization(x =>
                {
                    x.AddPolicyCustom(AuthorizationHelper.ClientAccessPolicy);
                    x.AddPolicyCustom(AuthorizationHelper.AdminAccessPolicy);
                    x.AddPolicyCustom(AuthorizationHelper.ProvisioningAccessPolicy);
                });

        private static void AddPolicyCustom(this AuthorizationOptions opts,  string policyname)
        {
            opts.AddPolicy(policyname, p => p.RequireClaim(AuthorizationHelper.GetClaimNameFor(policyname), "true"));
        }
    }
}
