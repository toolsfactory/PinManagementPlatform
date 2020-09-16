using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Initialization;
using PinPlatform.Common.Interfaces;
using PinPlatform.Common.Verifiers;
using PinPlatform.Common.DataStores;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace PinPlatform.Services.PinVerify
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddControllers();
            services.AddHealthChecks();
            services.AddSwaggerGen();
            services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(redisConfiguration);
            services.AddInitialization();
            services.AddTransient<IPinHashVerifier, PinHashVerifier>();
            services.AddTransient<IOpCoVerifier, OpCoVerifier>();

            // Registering PinRulesConfigurationStore three times to ensure that one singleton can be access with both interfaces required
            services.AddSingleton<PinRulesConfigurationStore>();
            services.AddSingleton<IAsyncInitializer>(x => x.GetRequiredService<PinRulesConfigurationStore>());
            services.AddSingleton<IRulesConfiguratonStore>(x => x.GetRequiredService<PinRulesConfigurationStore>());

            services.AddSingleton<PinDataStore>();
            services.AddSingleton<IAsyncInitializer>(x => x.GetRequiredService<PinDataStore>());
            services.AddSingleton<IPinDataStore>(x => x.GetRequiredService<PinDataStore>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();
            });
            app.UserRedisInformation();
        }
    }
}
