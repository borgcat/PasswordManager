using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace PasswordManager.Core.Tests.xUnit
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup()
        {
            var levelSwitch = new LoggingLevelSwitch();
            var log = new LoggerConfiguration()
              .MinimumLevel.ControlledBy(levelSwitch)
              .WriteTo.Seq("http://localhost:5341")
              .CreateLogger();

            log.Information("PasswordManager.Core.Tests.xUnit::Startup Ctor");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            
        }
    }
}
