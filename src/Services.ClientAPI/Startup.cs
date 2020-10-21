using AutoMapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Initialization;
using PinPlatform.Services.ClientAPI.Configuration;

namespace PinPlatform.Services.ClientAPI
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
            services.AddSwaggerCustom(Configuration);
            services.AddInitialization();
            services.AddControllers();
            services.AddHealthChecks();
            services.AddRedisCustom(Configuration);
            services.AddDomainAndInfrastructure();
            services.AddDatabase(Configuration);
            services.AddAutoMapper(typeof(Startup));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerCustom(env);

            if(env.IsDevelopment())
                app.UseMiddleware<Middleware.ExecutionTimeMiddleware>();

            app.UseMiddleware<Middleware.ExceptionMiddleware>();

            app.UseRouting();

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
