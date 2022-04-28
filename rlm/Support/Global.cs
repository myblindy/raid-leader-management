using rlm.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace rlm.Support
{
    static class Global
    {
        public static readonly JsonSerializerOptions JsonDeserializerOptions = new()
        {
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new EnumBitFieldJsonConverter<Roles>() },
        };
    }
}
