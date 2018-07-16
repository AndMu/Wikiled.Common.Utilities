using NUnit.Framework;
using Wikiled.Common.Utilities.Helpers;

namespace Wikiled.Common.Utilities.Tests.Helpers
{
    [TestFixture]
    public class ObjectClonerTests
    {
        [Test]
        public void CloneJson()
        {
            DataInstance instance = new DataInstance();
            instance.Text = "One";
            var result = instance.CloneJson();
            Assert.AreNotSame(instance, result);
            Assert.AreEqual(instance.Text, result.Text);
        }

        [Test]
        public void CloneJsonNull()
        {
            DataInstance instance = null;
            var result = instance.CloneJson();
            Assert.IsNull(result);
        }
    }
}