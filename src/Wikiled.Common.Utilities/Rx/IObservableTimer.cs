using System;

namespace Wikiled.Common.Utilities.Rx
{
    public interface IObservableTimer
    {
        IObservable<long> Daily(params TimeSpan[] times);
    }
}