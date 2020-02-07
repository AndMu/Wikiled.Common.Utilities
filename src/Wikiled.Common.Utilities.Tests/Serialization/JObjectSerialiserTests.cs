using System;
using NUnit.Framework;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Wikiled.Common.Testing.Utilities.Reflection;
using Wikiled.Common.Utilities.Helpers;
using Wikiled.Common.Utilities.Serialization;
using Wikiled.Common.Utilities.Tests.Helpers;

namespace Wikiled.Common.Utilities.Tests.Serialization
{
    [TestFixture]
    public class JObjectSerialiserTests
    {
        private JObjectSerialiser instance;

        private byte[] data;

        [SetUp]
        public void SetUp()
        {
            instance = CreateJObjectSerialiser();

            var subscription = new DataInstance();
            subscription.Text = "Test";
            subscription.Date = DateTime.Today;

            var json = JsonConvert.SerializeObject(subscription);
            data = Encoding.UTF8.GetBytes(json);
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

        [Test]
        public void Construct()
        {
            ConstructorHelper.ConstructorMustThrowArgumentNullException<JObjectSerialiser>(MemoryStreamInstances.MemoryStream);
        }

        private JObjectSerialiser CreateJObjectSerialiser()
        {
            return new JObjectSerialiser(MemoryStreamInstances.MemoryStream);
        }
    }
}
