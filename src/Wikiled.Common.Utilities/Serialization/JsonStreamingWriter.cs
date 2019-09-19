using Microsoft.IO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reactive.Disposables;
using System.Text;

namespace Wikiled.Common.Utilities.Serialization
{
    public class JsonStreamingWriter : IJsonStreamingWriter
    {
        private readonly JsonTextWriter writer;

        private int counter;

        private readonly CompositeDisposable disposable;

        private readonly RecyclableMemoryStreamManager memoryStream;

        public JsonStreamingWriter(StreamWriter streamWriter, RecyclableMemoryStreamManager memoryStream, params IDisposable[] disposables)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            this.memoryStream = memoryStream;
            writer = new JsonTextWriter(streamWriter);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartArray();
            disposable = new CompositeDisposable();
            disposable.Add(streamWriter);
            disposable.Add(writer);

            foreach (var item in disposables)
            {
                disposable.Add(item);
            }
        }

        public void WriteObject<T>(T instance)
        {
            counter++;

            using (var stream = memoryStream.GetStream())
            using (var streamWriter = new StreamWriter(stream: stream, encoding: Encoding.UTF8, bufferSize: 4096, leaveOpen: true)) // last parameter is important
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                var serializer = new JsonSerializer();
                serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, instance);
                streamWriter.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                string json = Encoding.UTF8.GetString(stream.ToArray());
                if (counter > 1)
                {
                    writer.WriteRaw($",{Environment.NewLine}");
                }

                writer.WriteRaw(json);
            }
        }

        public void Dispose()
        {
            writer.WriteEndArray();
            writer.Close();
            disposable?.Dispose();
        }
    }
}
