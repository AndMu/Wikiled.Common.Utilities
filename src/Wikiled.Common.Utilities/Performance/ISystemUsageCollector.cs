namespace Wikiled.Common.Utilities.Performance
{
    public interface ISystemUsageCollector
    {
        double TotalCpuUsed { get; }

        double PrivilegedCpuUsed { get; }

        double UserCpuUsed { get; }

        long WorkingSet { get; }

        long NonPagedSystemMemory { get; }

        long PagedMemory { get; }

        long PagedSystemMemory { get; }

        long PrivateMemory { get; }

        long VirtualMemoryMemory { get; }

        void CollectData();
    }
}