using System;
using System.Diagnostics;

namespace Wikiled.Common.Utilities.Performance
{
    public class SystemUsageCollector : ISystemUsageCollector
    {
        private readonly Process process = Process.GetCurrentProcess();
        private DateTime lastTimeStamp;
        private TimeSpan lastTotalProcessorTime = TimeSpan.Zero;
        private TimeSpan lastUserProcessorTime = TimeSpan.Zero;
        private TimeSpan lastPrivilegedProcessorTime = TimeSpan.Zero;

        public SystemUsageCollector()
        {
            lastTimeStamp = process.StartTime;
        }

        public double TotalCpuUsed { get; private set; }

        public double PrivilegedCpuUsed { get; private set; }

        public double UserCpuUsed { get; private set; }

        public long WorkingSet { get; private set; }

        public long NonPagedSystemMemory { get; private set; }

        public long PagedMemory { get; private set; }

        public long PagedSystemMemory { get; private set; }

        public long PrivateMemory { get; private set; }

        public long VirtualMemoryMemory { get; private set; }

        public void CollectData()
        {
            double totalCpuTimeUsed = process.TotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds;
            double privilegedCpuTimeUsed = process.PrivilegedProcessorTime.TotalMilliseconds - lastPrivilegedProcessorTime.TotalMilliseconds;
            double userCpuTimeUsed = process.UserProcessorTime.TotalMilliseconds - lastUserProcessorTime.TotalMilliseconds;

            lastTotalProcessorTime = process.TotalProcessorTime;
            lastPrivilegedProcessorTime = process.PrivilegedProcessorTime;
            lastUserProcessorTime = process.UserProcessorTime;

            double cpuTimeElapsed = (DateTime.UtcNow - lastTimeStamp).TotalMilliseconds * Environment.ProcessorCount;
            cpuTimeElapsed = cpuTimeElapsed < 0 ? 0 : cpuTimeElapsed;
            lastTimeStamp = DateTime.UtcNow;

            TotalCpuUsed = totalCpuTimeUsed * 100 / cpuTimeElapsed;
            PrivilegedCpuUsed = privilegedCpuTimeUsed * 100 / cpuTimeElapsed;
            UserCpuUsed = userCpuTimeUsed * 100 / cpuTimeElapsed;

            WorkingSet = process.WorkingSet64;
            NonPagedSystemMemory = process.NonpagedSystemMemorySize64;
            PagedMemory = process.PagedMemorySize64;
            PagedSystemMemory = process.PagedSystemMemorySize64;
            PrivateMemory = process.PrivateMemorySize64;
            VirtualMemoryMemory = process.VirtualMemorySize64;
        }
    }
}
