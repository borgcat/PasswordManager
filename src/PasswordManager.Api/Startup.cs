using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PasswordManager.Core;
using PasswordManager.Core.ConfigurationSettings;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Registry;
using Serilog;
using Serilog.Core;
using StructureMap;

namespace PasswordManager.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        private IContainer BootStrapStructureMap(IServiceCollection services)
        {
            var levelSwitch = new LoggingLevelSwitch();

            var log = new LoggerConfiguration()
              .MinimumLevel.ControlledBy(levelSwitch)
              .CreateLogger();

            var container = new Container();
            container.Configure(x =>
            {
                x.Scan(scan =>
                {
                    scan.WithDefaultConventions();
                });

                x.AddRegistry(new PasswordManagerRegistry(services));
            });

            return container;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddOptions()
                .AddMvc();

            services.Configure<AppSettingsConfiguration>(Configuration.GetSection("AppSettings"));
            IContainer container = BootStrapStructureMap(services);

            services.AddInstance(container.GetInstance<IRepository<PasswordEntity>>());
            services.AddInstance(container.GetInstance<IConfigurationSettings>());

            services.AddScoped<MasterKeyHeaderFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
