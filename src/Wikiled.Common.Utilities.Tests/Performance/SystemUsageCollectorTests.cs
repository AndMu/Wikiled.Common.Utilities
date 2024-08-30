using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Utilities.Performance;

namespace Wikiled.Common.Utilities.Tests.Performance
{
    [TestFixture]
    public class SystemUsageCollectorTests
    {
        private SystemUsageCollector instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateInstance();
        }

        [Test]
        public void Collect()
        {
            instance.Refresh();
            ClassicAssert.Greater(instance.PagedMemory, 0);
            ClassicAssert.Greater(instance.NonPagedSystemMemory, 0);
            ClassicAssert.Greater(instance.PrivateMemory, 0);
            ClassicAssert.Greater(instance.PagedSystemMemory, 0);
            ClassicAssert.Greater(instance.WorkingSet, 0);
            ClassicAssert.GreaterOrEqual(instance.UserCpuUsed, 0);
        }

        private SystemUsageCollector CreateInstance()
        {
            return new SystemUsageCollector();
        }
    }
}
