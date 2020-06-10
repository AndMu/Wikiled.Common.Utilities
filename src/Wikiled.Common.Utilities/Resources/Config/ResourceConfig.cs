using System.Collections.Generic;
using System.IO;

namespace Wikiled.Common.Utilities.Resources.Config
{
    public class ResourceConfig : ILocalDownload
    {
        public string Resources { get; set; }

        public Dictionary<string, LocationConfig> Targets { get; set; }

        public string GetFullPath(string name)
        {
            return Path.Combine(Resources ?? string.Empty, Targets[name].Local);
        }
    }
}
