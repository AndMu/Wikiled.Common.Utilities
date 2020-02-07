using System;
using System.IO;
using Microsoft.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wikiled.Common.Utilities.Serialization
{
    public class JObjectSerialiser : IJObjectSerialiser
    {
        private readonly RecyclableMemoryStreamManager memoryStream;

        public JObjectSerialiser(RecyclableMemoryStreamManager memoryStream)
        {
            this.memoryStream = memoryStream ?? throw new ArgumentNullException(nameof(memoryStream));
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
            return (JObject)JToken.ReadFrom(jsonTextReader);
        }
    }
}
