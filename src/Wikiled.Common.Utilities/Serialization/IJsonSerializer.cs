using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Wikiled.Common.Utilities.Serialization
{
    public interface IJsonSerializer
    {
        MemoryStream Serialize<T>(T instance, JsonSerializer custom = null);

        byte[] SerializeArray<T>(T instance, JsonSerializer custom = null);

        T Deserialize<T>(Stream stream, JsonSerializer custom = null);

        T Deserialize<T>(byte[] data, JsonSerializer custom = null);

        T Deserialize<T>(string json, JsonSerializer custom = null);

        JObject Deserialize(byte[] data);

        JObject Deserialize(Stream stream);

        T DeserializeJsonZip<T>(string fileName, JsonSerializer custom = null);

        Task SerializeJsonZip<T>(T instance, string fileName, JsonSerializer custom = null);
    }
}
