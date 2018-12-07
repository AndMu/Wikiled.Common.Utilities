using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Wikiled.Common.Utilities.Rx;

namespace Wikiled.Common.Utilities.Tests.Rx
{
    [TestFixture]
    public class ObservableExtensionsTests : ReactiveTest
    {
        private TestScheduler scheduler;

        [SetUp]
        public void SetUp()
        {
            scheduler = new TestScheduler();
        }

        [Test]
        public void CountSubscribers()
        {
            var observable = Observable.Interval(TimeSpan.FromSeconds(1), scheduler);
            int total = 0;
            observable = observable.CountSubscribers(item => total = item);
            Assert.AreEqual(0, total);
            var subs = observable.Subscribe(item => { });
            Assert.AreEqual(1, total);
            subs.Dispose();
            Assert.AreEqual(0, total);
        }

        [Test]
        public void WindowThrottle()
        {
            var stream = scheduler.CreateHotObservable(new Recorded<Notification<int>>(1, Notification.CreateOnNext(0)),
                                                        new Recorded<Notification<int>>(110, Notification.CreateOnNext(1)),
                                                        new Recorded<Notification<int>>(120, Notification.CreateOnNext(2)),
                                                        new Recorded<Notification<int>>(130, Notification.CreateOnNext(3)),
                                                        new Recorded<Notification<int>>(210, Notification.CreateOnNext(4)));
            var target = scheduler.CreateObserver<int>();
            stream.WindowThrottle(TimeSpan.FromTicks(100), scheduler).Subscribe(target);
            scheduler.AdvanceBy(1000);
            target.Messages.AssertEqual(
                OnNext<int>(1, item => item == 0),
                OnNext<int>(201, item => item == 3),
                OnNext<int>(301, item => item == 4));
        }
    }
}
