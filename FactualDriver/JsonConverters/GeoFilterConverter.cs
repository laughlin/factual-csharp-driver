using System;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.JsonConverters
{
    public class GeoFilterConverter: JsonConverter 
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as GeoFilter;

            writer.WriteStartObject();
            writer.WritePropertyName(filter.Shape);

            writer.WriteStartObject();
            writer.WritePropertyName(filter.Target);

            serializer.Serialize(writer, new [] { filter.Latitude, filter.Longitude});
            writer.WritePropertyName(filter.DistanceUnits);
            serializer.Serialize(writer, filter.Distance);

            writer.WriteEndObject();
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