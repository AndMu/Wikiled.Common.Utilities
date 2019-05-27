using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using Wikiled.Common.Extensions;
using Wikiled.Common.Utilities.Serialization;
using Wikiled.Common.Utilities.Tests.Helpers;

namespace Wikiled.Common.Utilities.Tests.Serialization
{
    [TestFixture]
    public class JsonSerializationExtensionTests
    {
        [Test]
        public async Task SerializeDeserialize()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "out");
            path.EnsureDirectoryExistence();
            path = Path.Combine(path, "data.zip");
            var instance = new DataInstance();
            instance.Text = "Test";
            await instance.SerializeJsonZip(path).ConfigureAwait(false);
            var result = await JsonSerializationExtension.DeserializeJsonZip<DataInstance>(path).ConfigureAwait(false);
            Assert.AreEqual(instance.Text, result.Text);
        }
    }
}
