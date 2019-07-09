using System;

namespace Wikiled.Common.Utilities.Performance
{
    public interface ISystemUsageMonitor : IDisposable
    {
        ISystemUsageCollector Collector { get; }

        ISystemUsageBucket UsageBucket { get; }

        event EventHandler Refreshed;

        void Start(TimeSpan refresh, TimeSpan maxPeriod);
    }
}