using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wikiled.Common.Utilities.Serialization
{
    public class BasicJsonSerializer : IJsonSerializer
    {
        private readonly RecyclableMemoryStreamManager memoryStream;

        private readonly JsonSerializer serializer = new JsonSerializer();

        public BasicJsonSerializer(RecyclableMemoryStreamManager memoryStream)
        {
            this.memoryStream = memoryStream ?? throw new ArgumentNullException(nameof(memoryStream));
        }

        public T Deserialize<T>(Stream stream, JsonSerializer custom = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using var sr = new StreamReader(stream);
            using var jr = new JsonTextReader(sr);
            return (custom ?? serializer).Deserialize<T>(jr);
        }

        public T Deserialize<T>(byte[] data, JsonSerializer custom = null)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using var stream = memoryStream.GetStream("Json", data, 0, data.Length);
            return Deserialize<T>(stream, custom);
        }

        public T Deserialize<T>(byte[] data, int offset, int count, JsonSerializer custom = null)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using var stream = memoryStream.GetStream("Json", data, offset, count);
            return Deserialize<T>(stream, custom);
        }

        public T Deserialize<T>(string json, JsonSerializer custom = null)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(json));
            }

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            var encode = Encoding.UTF8;
            var minLength = encode.GetByteCount(json);
            byte[] array = pool.Rent(minLength);
            encode.GetBytes(json, 0, json.Length, array, 0);

            try
            {
                using var stream = memoryStream.GetStream("Json", array, 0, minLength);
                return Deserialize<T>(stream, custom);
            }
            finally
            {
                pool.Return(array);
            }
        }

        public MemoryStream Serialize<T>(T instance, JsonSerializer custom = null)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var stream = memoryStream.GetStream("Json");

            using var streamWriter = new StreamWriter(stream: stream, encoding: Encoding.UTF8, bufferSize: 4096, leaveOpen: true); // last parameter is important
            using var jsonWriter = new JsonTextWriter(streamWriter);

            (custom ?? serializer).Serialize(jsonWriter, instance);
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public byte[] SerializeArray<T>(T instance, JsonSerializer custom = null)
        {
            using var resultStream = Serialize(instance, custom);
            using var outputStream = memoryStream.GetStream("Redis.Json");
            resultStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }

        public JObject Deserialize(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using var stream = memoryStream.GetStream("Json", data, 0, data.Length);

            return Deserialize(stream);
        }

        public JObject Deserialize(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);
            return (JObject) JToken.ReadFrom(jsonTextReader);
        }

        public T DeserializeJsonZip<T>(string fileName, JsonSerializer custom = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));
            }

            using (var compressedFileStream = File.OpenRead(fileName))
            using (var zipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            {
                return Deserialize<T>(zipStream, custom);
            }
        }

        public async Task SerializeJsonZip<T>(T instance, string fileName, JsonSerializer custom = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));
            }

            var compressedFileStream = File.Create(fileName);
            using (var zipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
            {
                using var stream = Serialize(instance, custom);
                await stream.CopyToAsync(zipStream).ConfigureAwait(false);
            }
        }
    }
}
