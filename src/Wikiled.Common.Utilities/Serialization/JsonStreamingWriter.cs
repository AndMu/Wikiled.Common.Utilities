using System;
using System.IO;
using System.IO.Compression;
using System.Reactive.Disposables;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wikiled.Common.Utilities.Serialization
{
    public class JsonStreamingWriter : IDisposable
    {
        private readonly StreamWriter streamWriter;

        private readonly JsonTextWriter writer;

        private int counter;

        private CompositeDisposable disposable;

        public JsonStreamingWriter(StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter ?? throw new ArgumentNullException(nameof(streamWriter));
            writer = new JsonTextWriter(streamWriter);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartArray();
            disposable = new CompositeDisposable();
            disposable.Add(streamWriter);
            disposable.Add(writer);
        }

        public static JsonStreamingWriter CreateJson(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            var streamWriter = new StreamWriter(path, false, Encoding.UTF8);
            streamWriter.AutoFlush = true;
            return new JsonStreamingWriter(streamWriter);
        }

        public static JsonStreamingWriter CreateCompressedJson(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            var compressedFileStream = File.Create(path);
            var zipStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
            var streamWriter = new StreamWriter(zipStream);
            streamWriter.AutoFlush = true;
            var instance = new JsonStreamingWriter(streamWriter);
            instance.disposable.Add(zipStream);
            instance.disposable.Add(compressedFileStream);
            return instance;
        }

        public void WriteObject<T>(T instance)
        {
            counter++;
            var json = JsonConvert.SerializeObject(instance, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            json = JToken.Parse(json).ToString(Formatting.Indented);
            lock (writer)
            {
                if (counter > 1)
                {
                    writer.WriteRaw($",{Environment.NewLine}");
                }

                writer.WriteRaw(json);
            }
        }

        public void Dispose()
        {
            lock (writer)
            {
                writer.WriteEndArray();
                writer.Close();
                disposable?.Dispose();
            }
        }
    }
}
