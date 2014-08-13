using FactualDriver.Filters;
using Newtonsoft.Json;
using System;

namespace FactualDriver.JsonConverters
{
    class RectConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as Rect;
            if (filter == null) throw new InvalidOperationException("RectConverter attribute is not applied to Rect class");

            writer.WriteStartObject();
            writer.WritePropertyName(Constants.WITHIN);

            writer.WriteStartObject();
            writer.WritePropertyName(filter.RectKey);
          
            serializer.Serialize(writer, new[] { new[]{filter.TopLeftLatitude, filter.TopLeftLLongitude}, new[] {filter.BottonRightLatitude, filter.BottonRightLongitude} });
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
