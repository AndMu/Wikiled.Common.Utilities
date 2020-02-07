using Microsoft.IO;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Serialization
{
    public class BasicJsonSerializer : IJsonSerializer
    {
        private readonly RecyclableMemoryStreamManager memoryStream;

        public BasicJsonSerializer(RecyclableMemoryStreamManager memoryStream)
        {
            this.memoryStream = memoryStream ?? throw new ArgumentNullException(nameof(memoryStream));
            Options = new JsonSerializerOptions();
        }

        public JsonSerializerOptions Options { get; }

        public ValueTask<T> Deserialize<T>(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            return JsonSerializer.DeserializeAsync<T>(stream, Options);
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return JsonSerializer.Deserialize<T>(data, Options);
        }

        public T Deserialize<T>(ArraySegment<byte> buffer)
        {
            return JsonSerializer.Deserialize<T>(buffer, Options);
        }

        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(json));
            }

            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public async Task<MemoryStream> Serialize<T>(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var stream = memoryStream.GetStream("Json");
            await JsonSerializer.SerializeAsync(stream, instance, Options).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public byte[] SerializeArray<T>(T instance)
        {
            return JsonSerializer.SerializeToUtf8Bytes(instance, Options);
        }

        public ValueTask<T> DeserializeJsonZip<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));
            }

            using (var compressedFileStream = File.OpenRead(fileName))
            using (var zipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            {
                return Deserialize<T>(zipStream);
            }
        }

        public async Task SerializeJsonZip<T>(T instance, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));
            }

            var compressedFileStream = File.Create(fileName);
            using (var zipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
            {
                using var stream = await Serialize(instance).ConfigureAwait(false);
                await stream.CopyToAsync(zipStream).ConfigureAwait(false);
            }
        }
    }
}
