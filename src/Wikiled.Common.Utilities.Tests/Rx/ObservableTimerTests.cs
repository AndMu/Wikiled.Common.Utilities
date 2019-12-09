using System;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using Wikiled.Common.Testing.Utilities.Reflection;
using Wikiled.Common.Utilities.Config;
using Wikiled.Common.Utilities.Rx;

namespace Wikiled.Common.Utilities.Tests.Rx
{
    [TestFixture]
    public class ObservableTimerTests : ReactiveTest
    {
        private Mock<IApplicationConfiguration> mockMarketConfiguration;

        private ObservableTimer instance;

        private TestScheduler testScheduler;

        [SetUp]
        public void Setup()
        {
            mockMarketConfiguration = new Mock<IApplicationConfiguration>();
            testScheduler = new TestScheduler();
            instance = CreateObservableTimer();
            mockMarketConfiguration.Setup(item => item.GetWorkDay(It.IsAny<DateTime>()))
                                   .Returns((DateTime item) => item);
            mockMarketConfiguration.Setup(item => item.Now).Returns(() => testScheduler.Now.UtcDateTime);
        }

        [Test]
        public void Daily()
        {
            var results = testScheduler.CreateObserver<long>();
            var nextHour = testScheduler.Now.UtcDateTime.Hour + 1;
            instance.Daily(TimeSpan.FromHours(nextHour)).Subscribe(results);
            testScheduler.AdvanceBy(TimeSpan.FromDays(3).Ticks);
            var due = TimeSpan.FromHours(1);
            results.Messages.AssertEqual(
                OnNext<long>(due.Ticks, instrument => true),
                OnNext<long>((due + TimeSpan.FromDays(1)).Ticks, instrument => true),
                OnNext<long>((due + TimeSpan.FromDays(2)).Ticks, instrument => true));
        }

        [Test]
        public void DailyMultiple()
        {
            var results = testScheduler.CreateObserver<long>();
            var nextHour = testScheduler.Now.UtcDateTime.Hour + 1;
            instance.Daily(TimeSpan.FromHours(nextHour), TimeSpan.FromHours(nextHour + 4), TimeSpan.FromHours(nextHour + 2)).Subscribe(results);
            testScheduler.AdvanceBy(TimeSpan.FromDays(3).Ticks);
            var due1 = TimeSpan.FromHours(1);
            var due2 = TimeSpan.FromHours(3);
            var due3 = TimeSpan.FromHours(5);
            results.Messages.AssertEqual(
                OnNext<long>(due1.Ticks, instrument => true),
                OnNext<long>(due2.Ticks, instrument => true),
                OnNext<long>(due3.Ticks, instrument => true),
                OnNext<long>((due1 + TimeSpan.FromDays(1)).Ticks, instrument => true),
                OnNext<long>((due2 + TimeSpan.FromDays(1)).Ticks, instrument => true),
                OnNext<long>((due3 + TimeSpan.FromDays(1)).Ticks, instrument => true),
                OnNext<long>((due1 + TimeSpan.FromDays(2)).Ticks, instrument => true),
                OnNext<long>((due2 + TimeSpan.FromDays(2)).Ticks, instrument => true),
                OnNext<long>((due3 + TimeSpan.FromDays(2)).Ticks, instrument => true));
        }

        [Test]
        public void Construct()
        {
            ConstructorHelper.ConstructorMustThrowArgumentNullException<ObservableTimer>();
        }

        private ObservableTimer CreateObservableTimer()
        {
            return new ObservableTimer(new NullLogger<ObservableTimer>(), mockMarketConfiguration.Object, testScheduler);
        }
    }
}
