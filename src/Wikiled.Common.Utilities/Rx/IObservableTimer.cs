using System;
using System.Reactive.Concurrency;

namespace Wikiled.Common.Utilities.Rx
{
    public interface IObservableTimer
    {
        IObservable<long> Daily(IScheduler scheduler, params TimeSpan[] times);
        IObservable<long> Daily(params TimeSpan[] times);
    }
}