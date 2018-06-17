using System;

namespace Wikiled.Common.Utilities.Config
{
    public interface IApplicationConfiguration
    {
        DateTime Now { get; }

        DateTime GetWorkDay(DateTime monitorDate);

        bool IsWorkDay(DateTime monitorDate);
    }
}