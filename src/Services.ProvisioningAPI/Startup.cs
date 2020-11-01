using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Initialization;
using PinPlatform.Services.Infrastructure.Authentication;
using PinPlatform.Services.Infrastructure.Authorization;
using PinPlatform.Services.Infrastructure.Configuration;
using PinPlatform.Services.Infrastructure.Middleware;
using PinPlatform.Services.ProvisioningAPI.Configuration;

namespace PinPlatform.Services.ProvisioningAPI
{
    public class Startup : PinPlatform.Services.Infrastructure.StartupBase
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISecurityKeyProvider, SymetricSecurityKeyProvider>();
            services.AddInitialization();
            services.AddControllers();
            services.AddApiVersioningCustom(Configuration);
            services.AddHealthChecks();
            services.AddRedisCustom(Configuration);
            services.AddDomainAndInfrastructure();
            services.AddDatabase(Configuration);
            services.AddCustomAuthentication(Configuration);
            services.AddCustomAuthorization(Configuration);

            if (Environment.IsDevelopment())
                services.AddSwaggerCustom(Configuration, "PinManagement Provisioning API", "Sample API exposed to OSS/BSS systems to manage pins");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerCustom(env, provider);
                app.UseMiddleware<ExecutionTimeMiddleware>();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
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
        }
    }
}
