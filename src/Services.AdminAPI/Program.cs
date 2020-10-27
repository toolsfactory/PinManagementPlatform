using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Initialization;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PinPlatform.Services.AdminAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            var exceptionless_url = Environment.GetEnvironmentVariable("EXCEPTIONLESS_URL");
            var exceptionless_apikey = Environment.GetEnvironmentVariable("EXCEPTIONLESS_APIKEY");
            var loggerconfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .WriteTo.File("/var/log/AdminApiService/log.txt", rollOnFileSizeLimit: true);

            if (String.IsNullOrEmpty(exceptionless_apikey))
            {
                Log.Logger = loggerconfig.CreateLogger();
                Log.Information("Main: Serilog logging initialized without Exceptionless support!");
            }
            else
            {
                Exceptionless.ExceptionlessClient.Default.Startup();
                Log.Logger = loggerconfig
                .WriteTo.Exceptionless(exceptionless_apikey, exceptionless_url)
                .CreateLogger();
                Log.Information($"Main: Serilog logging initialized with Exceptionless at {exceptionless_url}");
            }
            Log.Information("Main: Starting ClientApiService");
            try
            {
                Log.Information("Main: Creating HostBuilder");
                var builder = CreateHostBuilder(args);
                Log.Information("Main: Building Host");
                var host = builder.Build();
                Log.Information("Main: Initializing Host");
                host.Init();
                Log.Information("Main: Starting Host");
                host.Run();
                Log.Information("Main: Host started successfully");

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("opcoconfig.json",
                        optional: true,
                        reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
