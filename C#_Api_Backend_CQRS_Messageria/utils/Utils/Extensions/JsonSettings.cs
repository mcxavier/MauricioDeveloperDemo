using System;
using System.Collections.ObjectModel;
using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utils
{

    public static class JsonSettings
    {

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings{
            Formatting               = Formatting.Indented,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling        = DateParseHandling.None,
            NullValueHandling        = NullValueHandling.Ignore,
            DateFormatString         = "yyyy-MM-dd HH:mm",
        };

    }

    public class ParseStringConverter : JsonConverter
    {

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();

        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var value = serializer.Deserialize<string>(reader);

            if (long.TryParse(value, out var l)) {
                return l;
            }

            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null) {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (long) untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

    }

    public class DecimalConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer) {
                return token.ToObject<decimal>();
            }

            if (token.Type == JTokenType.String) {
                return decimal.Parse(token.ToString(), CultureInfo.GetCultureInfo("es-ES"));
            }

            if (token.Type == JTokenType.Null && objectType == typeof(decimal?)) {
                return null;
            }

            throw new JsonSerializationException($"Unexpected token type: {token.Type}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue($"{((decimal) value).ToString("N2", CultureInfo.InvariantCulture)}");
        }

    }

}