using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Wikiled.Common.Utilities.Performance
{
    public class SystemUsageMonitor : ISystemUsageMonitor
    {
        private IDisposable subscription;

        private readonly IScheduler scheduler;

        private TimeSpan maxPeriod;

        public SystemUsageMonitor(ISystemUsageCollector collector, ISystemUsageBucket usageBucket, IScheduler scheduler)
        {
            Collector = collector ?? throw new ArgumentNullException(nameof(collector));
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            UsageBucket = usageBucket ?? throw new ArgumentNullException(nameof(usageBucket));
        }

        public ISystemUsageCollector Collector { get; }

        public ISystemUsageBucket UsageBucket { get; }

        public void Start(TimeSpan refresh, TimeSpan maxPeriodScan)
        {
            if (subscription != null)
            {
                throw new Exception("Object is already active");
            }

            if (refresh.TotalMilliseconds < 500)
            {
                throw new ArgumentOutOfRangeException(nameof(refresh));
            }

            if (maxPeriodScan <= refresh)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPeriodScan));
            }

            maxPeriod = maxPeriodScan;
            subscription = Observable.Interval(refresh, scheduler).Subscribe(item => Refresh());
        }

        public void Dispose()
        {
            subscription?.Dispose();
        }

        private void Refresh()
        {
            Collector.Refresh();
            var usageData = new SystemUsageData();
            usageData.UserCpuUsed = Collector.UserCpuUsed;
            usageData.NonPagedSystemMemory = Collector.NonPagedSystemMemory;
            usageData.PagedSystemMemory = Collector.PagedSystemMemory;
            usageData.PrivateMemory = Collector.PrivateMemory;
            usageData.VirtualMemoryMemory = Collector.VirtualMemoryMemory;
            usageData.WorkingSet = Collector.WorkingSet;
            usageData.PrivilegedCpuUsed = Collector.PrivilegedCpuUsed;
            usageData.TotalCpuUsed = Collector.TotalCpuUsed;

            UsageBucket.Add(usageData);
            UsageBucket.RemoveOlder(maxPeriod);
            UsageBucket.Recalculate();
        }
    }
}
