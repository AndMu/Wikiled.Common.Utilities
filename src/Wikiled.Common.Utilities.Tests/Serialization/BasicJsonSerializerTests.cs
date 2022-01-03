using System;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;
using Wikiled.Common.Extensions;
using Wikiled.Common.Testing.Utilities.Reflection;
using Wikiled.Common.Utilities.Helpers;
using Wikiled.Common.Utilities.Serialization;
using Wikiled.Common.Utilities.Tests.Helpers;

namespace Wikiled.Common.Utilities.Tests.Serialization
{
    [TestFixture]
    public class BasicJsonSerializerTests
    {
        private BasicJsonSerializer instance;

        private byte[] data;

        private string json;

        private DataInstance subscription;

        [SetUp]
        public void SetUp()
        {
            instance = CreateBasicJsonSerializer();

            subscription = new DataInstance();
            subscription.Text = "Test";
            subscription.Date = DateTime.Today;

            json = JsonConvert.SerializeObject(subscription);
            data = Encoding.UTF8.GetBytes(json);
        }

        [Test]
        public void Construct()
        {
            ConstructorHelper.ConstructorMustThrowArgumentNullException(typeof(BasicJsonSerializer), type => type == typeof(RecyclableMemoryStreamManager) ? MemoryStreamInstances.MemoryStream : null);
        }

        [Test]
        public async Task Deserialize()
        {
            using (Stream stream = new MemoryStream(data))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var result = await instance.Deserialize<DataInstance>(stream);
                Assert.AreEqual("Test", result.Text);
            }
        }

        [Test]
        public void DeserializeFromBytes()
        {
            var result = instance.Deserialize<DataInstance>(data);
            Assert.AreEqual("Test", result.Text);
        }

        [Test]
        public void DeserializeFromString()
        {
            var result = instance.Deserialize<DataInstance>(json);
            Assert.AreEqual("Test", result.Text);
        }

        [Test]
        public async Task Serialize()
        {
            var stream = await instance.Serialize(subscription);
            var result = await instance.Deserialize<DataInstance>(stream);
            Assert.AreEqual("Test", result.Text);
        }

        [Test]
        public void SerializeArray()
        {
            var data = instance.SerializeArray(subscription);
            var result = instance.Deserialize<DataInstance>(data);
            Assert.AreEqual("Test", result.Text);
        }

        [Test]
        public async Task SerializeDeserialize()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "out");
            path.EnsureDirectoryExistence();
            path = Path.Combine(path, "data.zip");
            var dataInstance = new DataInstance();
            dataInstance.Text = "Test";
            await instance.SerializeJsonZip(dataInstance, path).ConfigureAwait(false);

            var result = await instance.DeserializeJsonZip<DataInstance>(path);
            Assert.AreEqual(dataInstance.Text, result.Text);
        }

        private BasicJsonSerializer CreateBasicJsonSerializer()
        {
            return new BasicJsonSerializer(MemoryStreamInstances.MemoryStream);
        }
    }
}