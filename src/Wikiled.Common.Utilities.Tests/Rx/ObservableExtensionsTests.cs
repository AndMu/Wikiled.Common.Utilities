using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Wikiled.Common.Utilities.Rx;

namespace Wikiled.Common.Utilities.Tests.Rx
{
    [TestFixture]
    public class ObservableExtensionsTests : ReactiveTest
    {
        private TestScheduler testScheduler;

        [SetUp]
        public void SetUp()
        {
            testScheduler = new TestScheduler();
        }

        [Test]
        public void CountSubscribers()
        {
            var observable = Observable.Interval(TimeSpan.FromSeconds(1), testScheduler);
            int total = 0;
            observable = observable.CountSubscribers(item => total = item);
            Assert.AreEqual(0, total);
            var subs = observable.Subscribe(item => { });
            Assert.AreEqual(1, total);
            subs.Dispose();
            Assert.AreEqual(0, total);
        }
    }
}
