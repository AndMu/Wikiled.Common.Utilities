using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreNotSame(instance, result);
            ClassicAssert.AreEqual(instance.Text, result.Text);
        }

        [Test]
        public void CloneJsonNull()
        {
            DataInstance instance = null;
            var result = instance.CloneJson();
            ClassicAssert.IsNull(result);
        }
    }
}