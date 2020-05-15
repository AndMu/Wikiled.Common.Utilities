using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Wikiled.Common.Utilities.Modules;

namespace Wikiled.Common.Utilities.Tests.Modules
{
    [TestFixture]
    public class ServiceCollectionExtensionsTests
    {
        private ServiceCollection collection;
        [SetUp]
        public void SetUp()
        {
            collection = new ServiceCollection();
        }

        [Test]
        public void AsyncFactory()
        {
            collection.AddSingleton("Test");
            collection.AddAsyncFactory(collection => Task.FromResult("Test"));
            var result = collection.BuildServiceProvider().GetService<string>();
            Assert.AreEqual("Test", result);
        }

        [Test]
        public void AsyncFactory2()
        {
            collection.AddSingleton("Test");
            collection.AddAsyncFactory<string>((collection, text) => Task.CompletedTask);
            var result = collection.BuildServiceProvider().GetService<string>();
            Assert.AreEqual("Test", result);
        }
    }
}
