using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Serialization
{
    public static class JsonSerializationExtension
    {
        public static async Task<T> DeserializeJsonZip<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));
            }

            using (var compressedFileStream = File.OpenRead(fileName))
            using (var zipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (var streamWriter = new StreamReader(zipStream))
            {
                var data = await streamWriter.ReadToEndAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(data);
            }
        }

        public static async Task SerializeJsonZip<T>(this T instance, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));
            }

            var compressedFileStream = File.Create(fileName);
            using (var zipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
            using (var streamWriter = new StreamWriter(zipStream))
            {
                streamWriter.AutoFlush = true;
                var data = JsonConvert.SerializeObject(instance);
                await streamWriter.WriteAsync(data).ConfigureAwait(false);
            }
        }
    }
}
