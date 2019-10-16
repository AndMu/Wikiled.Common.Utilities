using Microsoft.IO;

namespace Wikiled.Common.Utilities.Helpers
{
    public static class MemoryInstances
    {
        public static RecyclableMemoryStreamManager MemoryStream { get; } = new RecyclableMemoryStreamManager();
    }
}
