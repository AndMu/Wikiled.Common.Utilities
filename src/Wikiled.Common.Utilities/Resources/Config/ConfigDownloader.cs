using System;
using System.IO;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Resources.Config
{
    public class ConfigDownloader<T>
        where T : ILocalDownload
    {
        private readonly IDataDownloader downloader;

        private readonly T mainBlock;

        public ConfigDownloader(IDataDownloader downloader, T mainBlock)
        {
            this.downloader = downloader ?? throw new ArgumentNullException(nameof(downloader));
            this.mainBlock = mainBlock ?? throw new ArgumentNullException(nameof(mainBlock));
        }

        public async Task Download(Func<T, LocationConfig> configResolver, string location = null, bool always = false)
        {
            var config = configResolver(mainBlock);
            var path = Path.Combine(location ?? string.Empty, mainBlock.GetFullPath(configResolver));
            await downloader.DownloadFile(new Uri(config.Remote), path, always).ConfigureAwait(false);
        }
    }
}
