using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PasswordManager.Core.Interfaces;
using Xunit;
using Shouldly;

namespace PasswordManager.Core.Tests.xUnit
{
    public class EntityPersistenceTests : IClassFixture<StructureMapFixture>
    {
        private readonly StructureMapFixture _fixture;

        public EntityPersistenceTests(StructureMapFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void EntityPersistenceContainerTest()
        {
            var configurationSettings = _fixture.Container.GetInstance<IConfigurationSettings>();
            configurationSettings.MasterKey = "Test1234";
            IEntityPersistence<PasswordEntity> entityPersistence = _fixture.Container.GetInstance<IEntityPersistence<PasswordEntity>>();

            bool success = entityPersistence.PutList(new List<PasswordEntity>
            {
                new PasswordEntity
                {
                    CommonName = "Amazon",
                    Url = "www.amazon.com",
                    UserName = "dave@sample.com",
                    Password = "pass@word1"
                }
            });

            // assert
            success.ShouldBeTrue();
        }
    }
}
