using Microsoft.IO;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Wikiled.Common.Utilities.Serialization
{
    public class JsonStreamingWriterFactory : IJsonStreamingWriterFactory
    {
        private readonly RecyclableMemoryStreamManager memoryStream;

        public JsonStreamingWriterFactory(RecyclableMemoryStreamManager memoryStream)
        {
            this.memoryStream = memoryStream ?? throw new ArgumentNullException(nameof(memoryStream));
        }

        public IJsonStreamingWriter CreateJson(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            var streamWriter = new StreamWriter(path, false, Encoding.UTF8);
            streamWriter.AutoFlush = true;
            return new JsonStreamingWriter(streamWriter, memoryStream);
        }

        public IJsonStreamingWriter CreateCompressedJson(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            var compressedFileStream = File.Create(path);
            var zipStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
            var streamWriter = new StreamWriter(zipStream);
            streamWriter.AutoFlush = true;
            var instance = new JsonStreamingWriter(streamWriter, memoryStream, zipStream, compressedFileStream);
            return instance;
        }
    }
}
