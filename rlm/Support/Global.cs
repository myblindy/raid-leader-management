using rlm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
