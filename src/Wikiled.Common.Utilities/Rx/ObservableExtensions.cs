using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace Wikiled.Common.Utilities.Rx
{
    public static class ObservableExtensions
    {
        public static IObservable<T> WindowThrottle<T>(this IObservable<T> source, TimeSpan buffer, IScheduler scheduler)
        {
            var first = source.Take(1);
            var sequence = source.Window(() => Observable.Interval(buffer, scheduler)).SelectMany(x => x.TakeLast(1));
            return first.Concat(sequence);
        }


        public static IObservable<T> CountSubscribers<T>(this IObservable<T> source, Action<int> countChanged)
        {
            int count = 0;
            return Observable.Defer(() =>
            {
                count = Interlocked.Increment(ref count);
                countChanged(count);
                return source.Finally(() =>
                {
                    count = Interlocked.Decrement(ref count);
                    countChanged(count);
                });
            });
        }
    }
}
