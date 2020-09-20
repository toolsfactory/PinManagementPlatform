using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdditionalServiceCollectionServiceExtensions
    {
        #region Singleton Extensions
        public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<TService1>(x => x.GetRequiredService<TImplementation>());
            services.AddSingleton<TService2>(x => x.GetRequiredService<TImplementation>());
            return services;
        }

        public static IServiceCollection AddSingleton<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TImplementation : class, TService1, TService2, TService3
        {
            services.AddSingleton<TService1, TService2, TImplementation>();
            services.AddSingleton<TService3>(x => x.GetRequiredService<TImplementation>());
            return services;
        }

        public static IServiceCollection AddSingleton<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TService4 : class
            where TImplementation : class, TService1, TService2, TService3, TService4
        {
            services.AddSingleton<TService1, TService2, TService3, TImplementation>();
            services.AddSingleton<TService4>(x => x.GetRequiredService<TImplementation>());
            return services;
        }
        #endregion

        #region Transient Extension
        public static IServiceCollection AddTransient<TService1, TService2, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            services.AddTransient<TImplementation>();
            services.AddTransient<TService1>(x => x.GetRequiredService<TImplementation>());
            services.AddTransient<TService2>(x => x.GetRequiredService<TImplementation>());
            return services;
        }

        public static IServiceCollection AddTransient<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TImplementation : class, TService1, TService2, TService3
        {
            services.AddTransient<TService1, TService2, TImplementation>();
            services.AddTransient<TService3>(x => x.GetRequiredService<TImplementation>());
            return services;
        }

        public static IServiceCollection AddTransient<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TService4 : class
            where TImplementation : class, TService1, TService2, TService3, TService4
        {
            services.AddTransient<TService1, TService2, TService3, TImplementation>();
            services.AddTransient<TService4>(x => x.GetRequiredService<TImplementation>());
            return services;
        }
        #endregion

        #region Scoped Extension
        public static IServiceCollection AddScoped<TService1, TService2, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            services.AddScoped<TImplementation>();
            services.AddScoped<TService1>(x => x.GetRequiredService<TImplementation>());
            services.AddScoped<TService2>(x => x.GetRequiredService<TImplementation>());
            return services;
        }

        public static IServiceCollection AddScoped<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TImplementation : class, TService1, TService2, TService3
        {
            services.AddScoped<TService1, TService2, TImplementation>();
            services.AddScoped<TService3>(x => x.GetRequiredService<TImplementation>());
            return services;
        }

        public static IServiceCollection AddScoped<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TService4 : class
            where TImplementation : class, TService1, TService2, TService3, TService4
        {
            services.AddScoped<TService1, TService2, TService3, TImplementation>();
            services.AddScoped<TService4>(x => x.GetRequiredService<TImplementation>());
            return services;
        }

        #endregion
    }
}