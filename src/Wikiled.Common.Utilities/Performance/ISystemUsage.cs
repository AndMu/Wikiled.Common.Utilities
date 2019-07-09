namespace Wikiled.Common.Utilities.Performance
{
    public interface ISystemUsage
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
    }
}
