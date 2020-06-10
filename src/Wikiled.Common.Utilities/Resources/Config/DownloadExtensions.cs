using System;
using System.IO;

namespace Wikiled.Common.Utilities.Resources.Config
{
    public static class DownloadExtensions
    {
        public static string GetFullPath(this ILocalDownload download, Func<ILocalDownload, LocationConfig> config)
        {
            return Path.Combine(download.Resources ?? string.Empty, config(download).Local);
        }
    }
}
