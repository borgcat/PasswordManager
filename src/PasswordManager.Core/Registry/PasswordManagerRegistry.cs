using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.OptionsModel;
using PasswordManager.Core.Interfaces;
using Serilog;
using Serilog.Core;
using StructureMap.Graph;
using PasswordManager.Core.ConfigurationSettings;
using PasswordManager.Core.Encryption;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using PasswordManager.Core.Persistence;
using PasswordManager.Core.Repository;
using PasswordManager.Core.Serialization;

namespace PasswordManager.Core.Registry
{
    public class PasswordManagerRegistry : StructureMap.Registry
    {
        public PasswordManagerRegistry(IServiceCollection services)
        {
            var levelSwitch = new LoggingLevelSwitch();
            var log = new LoggerConfiguration()
              .MinimumLevel.ControlledBy(levelSwitch)
              .WriteTo.Seq("http://localhost:5341")
              .CreateLogger();
           
            Scan(scan =>
            {
                scan.WithDefaultConventions();
            });


            For<ILogger>().Use(log);
            For<IKeyPersistenceStrategy>().Singleton().Use<SynchronizedEncryptedKeyPersistenceStrategy>();
            For<IConfigurationSettings>().Singleton().Use<ConfigurationSettings.ConfigurationSettings>();
            For<IOptions<AppSettingsConfiguration>>().Use(services.BuildServiceProvider().GetService<IOptions<AppSettingsConfiguration>>());
            For<IEncryptionStrategy>().Use<AesEncryptionStrategy>();
            For<IEntityPersistence<PasswordEntity>>().Use<JsonFilePersistence<PasswordEntity>>();
            For<ISerialization<PasswordEntity>>().Use<FileSerialization<PasswordEntity>>();
            For<IRepository<PasswordEntity>>().Use<EntityRepository<PasswordEntity>>();
            For<IPasswordManagement>().Use<PasswordManagement.PasswordManagement>();

       
            //Policies.FillAllPropertiesOfType<IConfiguration>().Singleton().Use<Configuration.Configuration>();
        }
    }
}
