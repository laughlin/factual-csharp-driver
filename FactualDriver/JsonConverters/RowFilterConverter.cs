using FactualDriver.Filters;
using Newtonsoft.Json;
using System;

namespace FactualDriver.JsonConverters
{
    /// <summary>
    /// Converts RowFilter to json.
    /// </summary>
    public class RowFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as RowFilter;
            if (filter == null) throw new InvalidOperationException("RowFilterConverter attribute is not applied to RowFilter class");

            writer.WriteStartObject();
            writer.WritePropertyName(filter.FieldName);
            if (!string.IsNullOrWhiteSpace(filter.Operator))
            {
                writer.WriteStartObject();
                writer.WritePropertyName(filter.Operator);
            }

            serializer.Serialize(writer, filter.Value);
            if (!string.IsNullOrWhiteSpace(filter.Operator))
            {
                writer.WriteEndObject();
            }
                
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