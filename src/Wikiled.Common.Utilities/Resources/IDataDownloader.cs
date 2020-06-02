using System;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Resources
{
    public interface IDataDownloader
    {
        Task DownloadFile(Uri url, string output, bool always = false);
    }
}
