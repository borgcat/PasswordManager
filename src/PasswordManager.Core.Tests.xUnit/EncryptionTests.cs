using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordManager.Core.Interfaces;
using Serilog;
using Shouldly;
using Xunit;

namespace PasswordManager.Core.Tests.xUnit
{
    public class EncryptionTests : IClassFixture<StructureMapFixture>
    {
        private readonly StructureMapFixture _fixture;

        public EncryptionTests(StructureMapFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void KeyPersistenceStrategyContainerTest()
        {
            var configurationSettings = _fixture.Container.GetInstance<IConfigurationSettings>();
            configurationSettings.MasterKey = "Test1234";

            var keyPersistence = _fixture.Container.GetInstance<IKeyPersistenceStrategy>();

            string key = keyPersistence.AddOrRetrieveMasterKey("Test1234");
            key.ShouldBe("Bjw0Wm6oSuBBW5xk4nyfyVOcFWdQZMgmY7VtfMA0enA=");
        }
    }
}
