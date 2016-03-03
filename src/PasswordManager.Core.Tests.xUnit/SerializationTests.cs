using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordManager.Core.Interfaces;
using Shouldly;
using Xunit;

namespace PasswordManager.Core.Tests.xUnit
{
    public class SerializationTests : IClassFixture<StructureMapFixture>
    {
        private readonly StructureMapFixture _fixture;

        public SerializationTests(StructureMapFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void SerializationContainerTest()
        {
            var configurationSettings = _fixture.Container.GetInstance<IConfigurationSettings>();
            configurationSettings.MasterKey = "Test1234";

            ISerialization<PasswordEntity> entityPersistence = _fixture.Container.GetInstance<ISerialization<PasswordEntity>>();

            bool success = entityPersistence.SerializeObject(new PasswordEntity
            {
                CommonName = "Amazon",
                Url = "www.amazon.com",
                UserName = "dave@sample.com",
                Password = "pass@word1"
            });

            success.ShouldBe(true);
        }
    }
}
