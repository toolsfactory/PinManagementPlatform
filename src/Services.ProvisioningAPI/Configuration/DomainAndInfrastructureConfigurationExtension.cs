using Microsoft.Extensions.DependencyInjection;
using PinPlatform.Domain.Verifiers;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Processors;
using PinPlatform.Domain.Infrastructure;
using PinPlatform.Common;
using PinPlatform.Common.Implementrations;
using Microsoft.Extensions.Hosting.Initialization;
using PinPatform.Domain.Processors;

namespace PinPlatform.Services.ProvisioningAPI.Configuration
{
    public static class DomainAndInfrastructureConfigurationExtension
    {
        public static IServiceCollection AddDomainAndInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IPinHashVerifier, PinHashVerifier>();
            services.AddTransient<IOpCoVerifier, OpCoVerifier>();
            services.AddTransient<IPinChangeVerifier, PinChangeVerifier>();
            services.AddTransient<ILockPinProcessor, LockPinProcessor>();
            services.AddTransient<IDeletePinProcessor, DeletePinProcessor>();
            services.AddTransient<ICreatePinProcessor, CreatePinProcessor>();
            services.AddSingleton<IPinHashGenerator, PinHashGenerator>();
            services.AddSingleton<IPinCacheKeyGenerator, PinCacheKeyGenerator>();
            services.AddTransient<IPinRepository, PinRepository>();

            // Registering PinRulesConfigurationStore three times to ensure that one singleton can be access with both interfaces required
            services.AddSingleton<IAsyncInitializer, IRulesConfiguratonStore, PinRulesConfigurationStore>();
            return services;
        }
    }
}
