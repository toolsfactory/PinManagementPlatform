using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace PinPlatform.Services.ClientAPI.Configuration
{
    public static class RedisConfigurationExtension
    {
        public static IServiceCollection AddRedisCustom(this IServiceCollection services, IConfiguration config)
        {
            var redisConfiguration = config.GetSection("Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(redisConfiguration);
            return services;
        }
    }
}
