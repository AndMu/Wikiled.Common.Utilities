using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using Wikiled.Common.Testing.Utilities.Reflection;
using Wikiled.Common.Utilities.Helpers;
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
            config.Targets = new Dictionary<string, LocationConfig>
                             {
                                 {
                                     "One",
                                     new LocationConfig
                                     {
                                         Local = "1"
                                     }
                                 }
                             };

            var result = config.CloneJson();

        }
    }
}
