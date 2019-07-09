using System;

namespace Wikiled.Common.Utilities.Performance
{
    public interface ISystemUsageBucket
    {
        ISystemUsage Max { get; }
        ISystemUsage Average { get; }
        void Add(ISystemUsage data);
        void Recalculate();
        void RemoveOlder(TimeSpan timeSpan);
    }
}