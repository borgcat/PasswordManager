using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using StructureMap;

namespace PasswordManager.Core.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        private IContainer _container;

        [SetUp]
        public void Init()
        {
            var registry = new StructureMap.Registry();
            //registry.IncludeRegistry<PasswordManagerRegistry>();
            _container = new Container(registry);
        }

        [Test]
        public void TestMethod1()
        {
        }
    }
}
