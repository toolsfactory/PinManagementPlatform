using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PinPlatform.Services.Infrastructure
{
    public abstract class StartupBase
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        protected StartupBase(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
    }
}
