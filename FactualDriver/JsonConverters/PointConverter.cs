using System;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.JsonConverters
{
    public class PointConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as Point;
            writer.WriteStartObject();
            writer.WritePropertyName(filter.PointKey);
            serializer.Serialize(writer, new[] { filter.Latitude, filter.Longitude });
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}