namespace SilentMike.DietMenu.Core.WebApi.Extensions;

using System.Text.Json;
using System.Text.Json.Serialization;

internal static class JsonSerializerExtensions
{
    public static string ToIndentedIgnoreNullJson(this object self)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        return JsonSerializer.Serialize(self, options);
    }

    public static T? ToObject<T>(this string self, bool isJsonIndented = false)
    {
        if (string.IsNullOrWhiteSpace(self))
        {
            return default;
        }

        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = isJsonIndented,
        };

        return JsonSerializer.Deserialize<T>(self, options);
    }
}
