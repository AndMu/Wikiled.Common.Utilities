using NUnit.Framework;
using Wikiled.Common.Utilities.Resources.Config;

namespace Wikiled.Common.Utilities.Tests.Resources.Config
{
    [TestFixture]
    public class ResourceConfigTests
    {
        [Test]
        public void Serialise()
        {
            var config = new ResourceConfig();
            config.Resources = "Test";
            config.Location = new LocationConfig();
            config.Location.Local = "3";
            var result = config.GetFullPath(item => item.Location);
            Assert.AreEqual(@"Test\3", result);
        }
    }
}
