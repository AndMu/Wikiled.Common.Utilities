using Microsoft.IO;

namespace Wikiled.Common.Utilities.Helpers
{
    public static class MemoryStreamInstances
    {
        public static RecyclableMemoryStreamManager MemoryStream { get; } = new RecyclableMemoryStreamManager();
    }
}
