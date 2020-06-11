using System;
using System.IO;

namespace Wikiled.Common.Utilities.Resources.Config
{
    public static class DownloadExtensions
    {
        public static string GetFullPath<T>(this T download, Func<T, LocationConfig> config)
            where T : ILocalDownload
        {
            return Path.Combine(download.Resources ?? string.Empty, config(download).Local);
        }
    }
}
