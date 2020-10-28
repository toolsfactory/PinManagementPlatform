using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PinPlatform.Services.Infrastructure.Configuration
{
    public static class DatabaseConfigurationExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddEntityFrameworkMySql();
            services.AddDbContextPool<PinPlatform.Domain.Infrastructure.DB.DEMODBContext>(
                options => options.UseMySql("server=db;port=3306;user=root;password=test123;database=DEMODB",
                                opts => opts.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));
            return services;
        }
    }
}
