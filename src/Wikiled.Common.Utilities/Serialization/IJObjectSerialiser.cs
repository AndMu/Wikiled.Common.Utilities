using System.IO;
using Newtonsoft.Json.Linq;

namespace Wikiled.Common.Utilities.Serialization
{
    public interface IJObjectSerialiser
    {
        JObject Deserialize(byte[] data);

        JObject Deserialize(Stream stream);
    }
}