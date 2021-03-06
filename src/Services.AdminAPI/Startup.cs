using PinPlatform.Common.Implementrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PinPlatform.Common;
using PinPlatform.Services.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting.Initialization;
using PinPlatform.Services.Infrastructure.Authorization;
using PinPlatform.Services.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace PinPlatform.Services.AdminAPI
{
    public class Startup : PinPlatform.Services.Infrastructure.StartupBase
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment) 
            : base(configuration, environment)
        {  }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPinCacheKeyGenerator, PinCacheKeyGenerator>();
            services.AddSingleton<ISecurityKeyProvider, SymetricSecurityKeyProvider>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddInitialization();
            services.AddControllers();
            services.AddApiVersioningCustom(Configuration);
            services.AddDatabase(Configuration);
            services.AddRedisCustom(Configuration);
            services.AddCustomAuthentication(Configuration);
            services.AddCustomAuthorization(Configuration);

            if(Environment.IsDevelopment())
                services.AddSwaggerCustom(Configuration, "PinManagement Admin API", "A simple admin api for demo purposes");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UserRedisInformation();
                app.UseSwaggerCustom(env, provider);
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
