using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.OptionsModel;
using PasswordManager.Core.ConfigurationSettings;
using PasswordManager.Core.Interfaces;
using Serilog;
using Shouldly;
using Xunit;

namespace PasswordManager.Core.Tests.xUnit
{
    public class ContainerTests : IClassFixture<StructureMapFixture>
    {
        private readonly StructureMapFixture _fixture;

        public ContainerTests(StructureMapFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ContainerLoggerTest()
        {
            var logger = _fixture.Container.GetInstance<ILogger>();
            logger.Information("This is a test running");            
        }

        [Fact]
        public void ConfigurationSettingsTest()
        {
            IOptions<AppSettingsConfiguration> options = _fixture.Container.GetInstance<IOptions<AppSettingsConfiguration>>();

            options.Value.EncryptionSalt.ShouldNotBeNullOrEmpty();

            options.ShouldNotBeNull();
        }
    }
}
