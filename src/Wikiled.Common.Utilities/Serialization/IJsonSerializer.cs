using Newtonsoft.Json.Linq;
using System.IO;

namespace Wikiled.Common.Utilities.Serialization
{
    public interface IJsonSerializer
    {
        Stream Serialize<T>(T instance);

        T Deserialize<T>(Stream stream);

        T Deserialize<T>(byte[] data);

        T Deserialize<T>(string json);

        JObject Deserialize(byte[] data);

        JObject Deserialize(Stream stream);
    }
}
