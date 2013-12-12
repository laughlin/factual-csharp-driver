using FactualDriver.Filters;
using Newtonsoft.Json;
using System;

namespace FactualDriver.JsonConverters
{
    /// <summary>
    /// Converts FilterGroup to json.
    /// </summary>
    public class FilterGroupConverter : JsonConverter
    {
        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as FilterGroup;
            if (filter == null) throw new InvalidOperationException("FilterGroupConverter attribute is not applied to FilterGroup class");

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