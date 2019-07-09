using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Common.Utilities.Config;

namespace Wikiled.Common.Utilities.Performance
{
    public class SystemUsageBucket : ISystemUsageBucket
    {
        private readonly Queue<(DateTime Added, ISystemUsage Data)> measurements = new Queue<(DateTime, ISystemUsage)>();

        private SystemUsageData max = new SystemUsageData();

        private SystemUsageData average = new SystemUsageData();

        private readonly IApplicationConfiguration configuration;

        public SystemUsageBucket(IApplicationConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Recalculate();
        }

        public ISystemUsage Max => max;

        public ISystemUsage Average => average;

        public void Add(ISystemUsage data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            measurements.Enqueue((configuration.Now, data));
        }

        public void Recalculate()
        {
            if (measurements.Count == 0)
            {
                max = new SystemUsageData();
                average = new SystemUsageData();
                return;
            }

            max.UserCpuUsed = measurements.Max(item => item.Data.UserCpuUsed);
            max.NonPagedSystemMemory = measurements.Max(item => item.Data.NonPagedSystemMemory);
            max.PagedSystemMemory = measurements.Max(item => item.Data.PagedSystemMemory);
            max.PrivateMemory = measurements.Max(item => item.Data.PrivateMemory);
            max.VirtualMemoryMemory = measurements.Max(item => item.Data.VirtualMemoryMemory);
            max.WorkingSet = measurements.Max(item => item.Data.WorkingSet);
            max.PrivilegedCpuUsed = measurements.Max(item => item.Data.PrivilegedCpuUsed);
            max.TotalCpuUsed = measurements.Max(item => item.Data.TotalCpuUsed);

            average.UserCpuUsed = measurements.Average(item => item.Data.UserCpuUsed);
            average.NonPagedSystemMemory = (long)measurements.Average(item => item.Data.NonPagedSystemMemory);
            average.PagedSystemMemory = (long)measurements.Average(item => item.Data.PagedSystemMemory);
            average.PrivateMemory = (long)measurements.Average(item => item.Data.PrivateMemory);
            average.VirtualMemoryMemory = (long)measurements.Average(item => item.Data.VirtualMemoryMemory);
            average.WorkingSet = (long)measurements.Average(item => item.Data.WorkingSet);
            average.PrivilegedCpuUsed = measurements.Average(item => item.Data.PrivilegedCpuUsed);
            average.TotalCpuUsed = measurements.Average(item => item.Data.TotalCpuUsed);
        }

        public void RemoveOlder(TimeSpan timeSpan)
        {
            var cutOff = configuration.Now.Subtract(timeSpan);
            while (measurements.Count > 0 && measurements.Peek().Added < cutOff)
            {
                measurements.Dequeue();
            }
        }
    }
}
