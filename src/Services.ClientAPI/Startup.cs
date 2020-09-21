using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Implementrations;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Initialization;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Common.Repositories;
using PinPlatform.Common.Verifiers;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

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
            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddControllers();
            services.AddHealthChecks();
            services.AddSwaggerGen();
            services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(redisConfiguration);
            services.AddInitialization();
            services.AddTransient<IPinHashVerifier, PinHashVerifier>();
            services.AddTransient<IOpCoVerifier, OpCoVerifier>();

            // Registering PinRulesConfigurationStore three times to ensure that one singleton can be access with both interfaces required
            services.AddSingleton<IAsyncInitializer, IRulesConfiguratonStore, PinRulesConfigurationStore>();

            services.AddTransient<IPinRepository, PinRepository>();
            services.AddEntityFrameworkMySql();
            services.AddDbContextPool<PinPlatform.Common.DEMODBContext>(
                options => options.UseMySql("server=db;port=3306;user=root;password=test123;database=DEMODB"));
            services.AddSingleton<IPinCacheKeyGenerator, PinCacheKeyGenerator>();

            services.AddAutoMapper(typeof(Startup));
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
