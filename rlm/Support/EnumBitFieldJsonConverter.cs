using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rlm.Support
{
    class EnumBitFieldJsonConverter<T> : JsonConverter<T>
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int outVal = 0;
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    outVal += (int)Enum.Parse(typeof(T), Encoding.UTF8.GetString(reader.ValueSpan), true);
                    reader.Read();
                }
            }
            return (T)(object)outVal;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
