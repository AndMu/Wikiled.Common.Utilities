using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using Wikiled.Common.Extensions;
using Wikiled.Common.Utilities.Serialization;
using Wikiled.Common.Utilities.Tests.Helpers;

namespace Wikiled.Common.Utilities.Tests.Serialization
{
    [TestFixture]
    public class JsonStreamingWriterTests
    {
        private string path;

        [SetUp]
        public void Setup()
        {
            path = Path.Combine(TestContext.CurrentContext.TestDirectory, "out");
            path.EnsureDirectoryExistence();
        }

        [Test]
        public void CreateJson()
        {
            path = Path.Combine(path, "data.json");
            using (var writer = JsonStreamingWriter.CreateJson(path))
            {
                writer.WriteObject(new DataInstance());
                writer.WriteObject(new DataInstance());
            }

            var result = JsonConvert.DeserializeObject<DataInstance[]>(File.ReadAllText(path));
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void CreateCompressedJson()
        {
            path.EnsureDirectoryExistence();
            path = Path.Combine(path, "data.zip");
            using (var writer = JsonStreamingWriter.CreateCompressedJson(path))
            {
                writer.WriteObject(new DataInstance());
                writer.WriteObject(new DataInstance());
            }
        }
    }
}
