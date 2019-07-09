using Moq;
using NUnit.Framework;
using System;
using Microsoft.Reactive.Testing;
using Wikiled.Common.Utilities.Performance;

namespace Wikiled.Common.Utilities.Tests.Performance
{
    [TestFixture]
    public class SystemUsageMonitorTests
    {
        private Mock<ISystemUsageCollector> mockSystemUsageCollector;
        private Mock<ISystemUsageBucket> mockSystemUsageBucket;
        private TestScheduler scheduler;

        private SystemUsageMonitor instance;

        [SetUp]
        public void SetUp()
        {
            mockSystemUsageCollector = new Mock<ISystemUsageCollector>();
            mockSystemUsageBucket = new Mock<ISystemUsageBucket>();
            scheduler = new TestScheduler();
            instance = CreateInstance();
        }

        [Test]
        public void DisposeNew()
        {
            instance.Dispose();
        }

        [Test]
        public void StartArguments()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => instance.Start(TimeSpan.FromMilliseconds(1), TimeSpan.FromMinutes(2)));
            Assert.Throws<ArgumentOutOfRangeException>(() => instance.Start(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)));

            instance.Start(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2));
            Assert.Throws<Exception>(() => instance.Start(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2)));
        }

        [Test]
        public void Start()
        {
            int times = 0;
            instance.Refreshed += (s, a) => times++;
            instance.Start(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2));
            scheduler.AdvanceBy(TimeSpan.FromMinutes(1).Ticks);
            mockSystemUsageCollector.Verify(item => item.Refresh(), Times.Once);
            mockSystemUsageBucket.Verify(item => item.Add(It.IsAny<ISystemUsage>()), Times.Once());
            mockSystemUsageBucket.Verify(item => item.Recalculate(), Times.Once());
            mockSystemUsageBucket.Verify(item => item.RemoveOlder(TimeSpan.FromMinutes(2)), Times.Once());
            Assert.AreEqual(1, times);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new SystemUsageMonitor(null, mockSystemUsageBucket.Object, scheduler));
            Assert.Throws<ArgumentNullException>(() => new SystemUsageMonitor(mockSystemUsageCollector.Object, null, scheduler));
            Assert.Throws<ArgumentNullException>(() => new SystemUsageMonitor(mockSystemUsageCollector.Object, mockSystemUsageBucket.Object, null));

        }

        private SystemUsageMonitor CreateInstance()
        {
            return new SystemUsageMonitor(mockSystemUsageCollector.Object, mockSystemUsageBucket.Object, scheduler);
        }
    }
}
