using System.Collections.Generic;

namespace Wikiled.Common.Utilities.Resources.Config
{
    public interface ILocalDownload
    {
        string Resources { get; }

        Dictionary<string, LocationConfig> Targets { get; }

        string GetFullPath(string name);
    }
}
