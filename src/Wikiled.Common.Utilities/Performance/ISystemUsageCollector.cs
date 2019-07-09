namespace Wikiled.Common.Utilities.Performance
{
    public interface ISystemUsageCollector : ISystemUsage
    {
        void Refresh();
    }
}