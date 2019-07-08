using Moq;
using NUnit.Framework;
using Wikiled.Common.Utilities.Performance;

namespace Wikiled.Common.Utilities.Tests.Performance
{
    [TestFixture]
    public class SystemUsageCollectorExtensionTests
    {
        [TestCase(120, 55.5555, 44.444, "Service Monitoring. Working Set: 0.12 KB Total CPU Used: 55.56 User CPU: 44.44")]
        [TestCase(12000, 55.5555, 44.444, "Service Monitoring. Working Set: 11.72 KB Total CPU Used: 55.56 User CPU: 44.44")]
        [TestCase(1024 * 1024 * 56, 55.5555, 44.444, "Service Monitoring. Working Set: 56.00 MB Total CPU Used: 55.56 User CPU: 44.44")]
        [TestCase(1024L * 1024 * 1024 * 56, 55.5555, 44.444, "Service Monitoring. Working Set: 56.00 GB Total CPU Used: 55.56 User CPU: 44.44")]
        public void GetBasic(long memory, double cpu, double userCpu, string expected)
        {
            var collector = new Mock<ISystemUsageCollector>();
            collector.Setup(item => item.WorkingSet).Returns(memory);
            collector.Setup(item => item.TotalCpuUsed).Returns(cpu);
            collector.Setup(item => item.UserCpuUsed).Returns(userCpu);
            Assert.AreEqual(expected, collector.Object.GetBasic());
        }
    }
}
