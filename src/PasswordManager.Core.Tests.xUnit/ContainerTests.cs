using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
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
        public void ContainerTest()
        {
            var logger = _fixture.Container.GetInstance<ILogger>();
            logger.Information("This is a test running");            
        }
    }
}
