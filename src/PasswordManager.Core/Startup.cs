using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace PasswordManager.Core
{
    public class Startup
    {
        private readonly IApplicationEnvironment _appenv;

        public Startup(IApplicationEnvironment appenv)
        {
            _appenv = appenv;

            string sAppPath = appenv.ApplicationBasePath;
            string sRootPath = Path.GetFullPath(Path.Combine(sAppPath, @"..\..\"));
            string sBinFolderPath = @"artifacts\bin\" + appenv.ApplicationName;
            string sBinPath = Path.Combine(sRootPath, sBinFolderPath);
        }

        void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(_appenv.ApplicationBasePath)
                //.AddJsonFile($"config.{env.EnvironmentName}.json", optional: true),
                .AddJsonFile("appsettings.json");
        }
    }
}
