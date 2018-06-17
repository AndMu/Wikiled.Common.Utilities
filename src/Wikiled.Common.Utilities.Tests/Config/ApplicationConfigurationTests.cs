using System;
using NUnit.Framework;
using Wikiled.Common.Utilities.Config;

namespace Wikiled.Common.Utilities.Tests.Config
{
    [TestFixture]
    public class ApplicationConfigurationTests
    {
        private ApplicationConfiguration instance;

        [SetUp]
        public void Setup()
        {
            instance = new ApplicationConfiguration();
        }

        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(7, false)]
        public void CanMonitorMarket(int day, bool expected)
        {
            var dateTime = new DateTime(2012, 01, day);
            var result = instance.IsWorkDay(dateTime);
            Assert.AreEqual(expected, result);
        }

        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(7, 9)]
        public void GetWorkDay(int day, int workday)
        {
            var dateTime = new DateTime(2012, 01, day);
            var result = instance.GetWorkDay(dateTime);
            Assert.AreEqual(workday, result.Day);
        }
    }
}
