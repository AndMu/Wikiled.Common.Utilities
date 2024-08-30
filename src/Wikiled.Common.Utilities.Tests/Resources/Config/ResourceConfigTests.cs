using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(@"Test\3", result);
        }
    }
}
