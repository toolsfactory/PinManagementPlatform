using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Initialization;
using PinPlatform.Services.ClientAPI.Configuration;
using PinPlatform.Services.Infrastructure.Authentication;
using PinPlatform.Services.Infrastructure.Authorization;
using PinPlatform.Services.Infrastructure.Configuration;
using PinPlatform.Services.Infrastructure.Middleware;

namespace PinPlatform.Services.ClientAPI
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
            services.AddHealthChecks();
            services.AddRedisCustom(Configuration);
            services.AddDomainAndInfrastructure();
            services.AddDatabase(Configuration);
            services.AddCustomAuthentication(Configuration);
            services.AddCustomAuthorization(Configuration);

            if (Environment.IsDevelopment())
                services.AddSwaggerCustom(Configuration, "v2", "PinManagement Client API", "Sample API exposed to clients to manage & verify pins");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerCustom(env);
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
