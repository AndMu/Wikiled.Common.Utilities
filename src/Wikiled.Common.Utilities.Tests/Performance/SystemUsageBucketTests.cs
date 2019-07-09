using Moq;
using NUnit.Framework;
using System;
using Wikiled.Common.Utilities.Config;
using Wikiled.Common.Utilities.Performance;

namespace Wikiled.Common.Utilities.Tests.Performance
{
    [TestFixture]
    public class SystemUsageBucketTests
    {
        private SystemUsageData data;

        private SystemUsageBucket instance;

        private Mock<IApplicationConfiguration> configuration;

        [SetUp]
        public void SetUp()
        {
            configuration = new Mock<IApplicationConfiguration>();
            configuration.Setup(item => item.Now).Returns(DateTime.UtcNow);
            instance = CreateInstance();
        }

        [Test]
        public void Arguments()
        {
            Assert.Throws<ArgumentNullException>(() => instance.Add(null));
        }

        [Test]
        public void Add()
        {
            data = new SystemUsageData();
            data.UserCpuUsed = 40;
            data.WorkingSet = 200;
            instance.Add(data);
            instance.Recalculate();

            Assert.AreEqual(40, instance.Average.UserCpuUsed);
            Assert.AreEqual(200, instance.Average.WorkingSet);

            Assert.AreEqual(40, instance.Max.UserCpuUsed);
            Assert.AreEqual(200, instance.Max.WorkingSet);


            data = new SystemUsageData();
            data.UserCpuUsed = 20;
            data.WorkingSet = 100;
            instance.Add(data);
            instance.Recalculate();

            Assert.AreEqual(30, instance.Average.UserCpuUsed);
            Assert.AreEqual(150, instance.Average.WorkingSet);

            Assert.AreEqual(40, instance.Max.UserCpuUsed);
            Assert.AreEqual(200, instance.Max.WorkingSet);
        }

        [Test]
        public void RemoveOlder()
        {
            data = new SystemUsageData();
            data.UserCpuUsed = 40;
            data.WorkingSet = 200;
            instance.Add(data);
            instance.Recalculate();

            Assert.AreEqual(40, instance.Average.UserCpuUsed);
            Assert.AreEqual(200, instance.Average.WorkingSet);

            configuration.Setup(item => item.Now).Returns(DateTime.UtcNow.AddHours(1));
            instance.RemoveOlder(TimeSpan.FromMinutes(1));
            instance.Recalculate();

            Assert.AreEqual(0, instance.Average.UserCpuUsed);
            Assert.AreEqual(0, instance.Average.WorkingSet);

            Assert.AreEqual(0, instance.Max.UserCpuUsed);
            Assert.AreEqual(0, instance.Max.WorkingSet);
        }

        [Test]
        public void Empty()
        {
            Assert.AreEqual(0, instance.Average.UserCpuUsed);
            Assert.AreEqual(0, instance.Average.WorkingSet);

            Assert.AreEqual(0, instance.Max.UserCpuUsed);
            Assert.AreEqual(0, instance.Max.WorkingSet);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new SystemUsageBucket(null));
        }

        private SystemUsageBucket CreateInstance()
        {
            return new SystemUsageBucket(configuration.Object);
        }
    }
}
