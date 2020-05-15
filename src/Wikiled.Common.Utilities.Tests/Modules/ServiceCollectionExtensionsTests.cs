using System;
using System.Threading.Tasks;
using System.Xml.Schema;
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
        public async Task AsyncFactory()
        {
            collection.AddAsyncFactory(collection => Task.FromResult("Test"));
            var provider = collection.BuildServiceProvider();
            var result = await provider.GetService<IAsyncServiceFactory<string>>().GetService();
            Assert.AreEqual("Test", result);
        }

        [Test]
        public async Task AsyncFactory2()
        {
            collection.AddSingleton("Test");
            collection.AddAsyncFactory<string>((collection, text) => Task.CompletedTask);
            var provider = collection.BuildServiceProvider();
            var result = await provider.GetService<IAsyncServiceFactory<string>>().GetService();
            Assert.AreEqual("Test", result);
        }

        [Test]
        public async Task AsyncFactoryRefresh()
        {
            var total = 0;
            collection.AddAsyncFactory(collection => Task.FromResult((object)total++));
            var provider = collection.BuildServiceProvider();
            var result = await provider.GetService<IAsyncServiceFactory<object>>().GetService();
            Assert.AreEqual(0, result);
            result = await provider.GetService<IAsyncServiceFactory<object>>().GetService();
            Assert.AreEqual(0, result);
            result = await provider.GetService<IAsyncServiceFactory<object>>().GetService(true);
            Assert.AreEqual(1, result);
        }
    }
}
