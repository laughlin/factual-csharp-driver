using System;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.JsonConverters
{
    public class ConditionalRowFilterConverter : JsonConverter
    {
        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as ConditionalRowFilter;

            writer.WriteStartObject();
            writer.WritePropertyName(filter.Operator);
            serializer.Serialize(writer, filter.RowFilters);
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

        #endregion
    }
}