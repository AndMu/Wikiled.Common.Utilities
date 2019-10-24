using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Serialization
{
    public interface IJsonSerializer
    {
        Stream Serialize<T>(T instance);

        byte[] SerializeArray<T>(T instance);

        T Deserialize<T>(Stream stream);

        T Deserialize<T>(byte[] data);

        T Deserialize<T>(string json);

        JObject Deserialize(byte[] data);

        JObject Deserialize(Stream stream);

        T DeserializeJsonZip<T>(string fileName);

        Task SerializeJsonZip<T>(T instance, string fileName);
    }
}
