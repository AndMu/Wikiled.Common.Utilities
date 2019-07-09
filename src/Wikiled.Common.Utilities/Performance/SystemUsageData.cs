namespace Wikiled.Common.Utilities.Performance
{
    public class SystemUsageData : ISystemUsage
    {
        public double TotalCpuUsed { get; set; }

        public double PrivilegedCpuUsed { get; set; }

        public double UserCpuUsed { get; set; }

        public long WorkingSet { get; set; }

        public long NonPagedSystemMemory { get; set; }

        public long PagedMemory { get; set; }

        public long PagedSystemMemory { get; set; }

        public long PrivateMemory { get; set; }

        public long VirtualMemoryMemory { get; set; }
    }
}
