using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Text;
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

            json = JsonConvert.SerializeObject(subscription);
            data = Encoding.UTF8.GetBytes(json);
        }

        [Test]
        public void Construct()
        {
            ConstructorHelper.ConstructorMustThrowArgumentNullException(typeof(BasicJsonSerializer), MemoryInstances.MemoryStream);
        }

        [Test]
        public void Deserialize()
        {
            using (Stream stream = new MemoryStream(data))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var result = instance.Deserialize<DataInstance>(stream);
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
        public void Serialize()
        {
            var stream = instance.Serialize(subscription);
            var result = instance.Deserialize<DataInstance>(stream);
            Assert.AreEqual("Test", result.Text);
        }

        [Test]
        public void DeserializeJObject()
        {
            var result = instance.Deserialize(data).ToObject<DataInstance>();
            Assert.AreEqual("Test", result.Text);
        }

        [Test]
        public void DeserializeJObjectFromBytes()
        {
            using (Stream stream = new MemoryStream(data))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var result = instance.Deserialize(stream).ToObject<DataInstance>();
                Assert.AreEqual("Test", result.Text);
            }
        }

        private BasicJsonSerializer CreateBasicJsonSerializer()
        {
            return new BasicJsonSerializer(MemoryInstances.MemoryStream);
        }
    }
}