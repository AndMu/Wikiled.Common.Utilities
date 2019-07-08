using System;
using Moq;
using NUnit.Framework;
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
            Assert.Greater(instance.PagedMemory, 0);
            Assert.Greater(instance.NonPagedSystemMemory, 0);
            Assert.Greater(instance.PrivateMemory, 0);
            Assert.Greater(instance.PagedSystemMemory, 0);
            Assert.Greater(instance.WorkingSet, 0);
            Assert.GreaterOrEqual(instance.UserCpuUsed, 0);
        }

        private SystemUsageCollector CreateInstance()
        {
            return new SystemUsageCollector();
        }
    }
}
