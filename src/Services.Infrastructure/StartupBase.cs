using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
