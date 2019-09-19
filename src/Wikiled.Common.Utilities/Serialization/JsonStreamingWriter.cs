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

        private readonly JsonSerializer jsonSerializer;

        public JsonStreamingWriter(StreamWriter streamWriter, RecyclableMemoryStreamManager memoryStream, params IDisposable[] disposables)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }


            jsonSerializer = new JsonSerializer();
            jsonSerializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            jsonSerializer.Formatting = Formatting.Indented;

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
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 4096, true))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonWriter, instance);
                streamWriter.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                if (counter > 1)
                {
                    writer.WriteRaw($",{Environment.NewLine}");
                }

                using (var reader = new StreamReader(stream))
                {
                    writer.WriteRaw(reader.ReadToEnd());
                }
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
