using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wikiled.Common.Utilities.Helpers;

public static class ObjectCloner
{
    public static T CloneJson<T>(this T source)
    {
        if (ReferenceEquals(source, null))
        {
            return default(T);
        }

        var deserializeSettings = new JsonSerializerOptions { PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace };
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source), deserializeSettings);
    }
}