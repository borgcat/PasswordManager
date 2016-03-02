using System;
using PasswordManager.Core.Registry;
using StructureMap;

namespace PasswordManager.Core.Tests.xUnit
{
    public class StructureMapFixture : IDisposable
    {
        public StructureMapFixture()
        {
            var registry = new StructureMap.Registry();
            registry.IncludeRegistry<PasswordManagerRegistry>();
            Container = new Container(registry);
        }

        public void Dispose()
        {
        }

        public IContainer Container { get; private set; }
    }
}