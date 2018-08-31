using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Resources
{
    public class DataDownloader
    {
        private readonly ILogger log;

        public DataDownloader(ILoggerFactory logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            log = logger.CreateLogger<DataDownloader>();
        }

        public async Task DownloadFile(Uri url, string output, bool always = false)
        {
            log.LogInformation("Downloading <{0}> to <{1}>", url, output);
            if (Directory.Exists(output))
            {
                log.LogInformation("Resources folder <{0} found.", output);
                if (!always)
                {
                    return;
                }
            }

            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    UnzipFromStream(stream, output);
                }
            }
        }

        private void UnzipFromStream(Stream zipStream, string outFolder)
        {
            ZipInputStream zipInputStream = new ZipInputStream(zipStream);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();
            while (zipEntry != null)
            {
                string entryFileName = zipEntry.Name;
                log.LogInformation("Unpacking [{0}]", entryFileName);

                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.
                byte[] buffer = new byte[4096];     // 4K is optimum

                // Manipulate the output filename here as desired.
                string fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                {
                    Directory.CreateDirectory(directoryName);
                }

                // Skip directory entry
                string fileName = Path.GetFileName(fullZipToPath);
                if (fileName.Length == 0)
                {
                    zipEntry = zipInputStream.GetNextEntry();
                    continue;
                }

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                }

                zipEntry = zipInputStream.GetNextEntry();
            }
        }
    }
}
