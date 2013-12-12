using FactualDriver.Filters;
using Newtonsoft.Json;
using System;

namespace FactualDriver.JsonConverters
{
    /// <summary>
    /// Converts Point to json.
    /// </summary>
    public class PointConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as Point;
            if (filter == null) throw new InvalidOperationException("PointConverter attribute is not applied to Point class");

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