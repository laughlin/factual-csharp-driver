using FactualDriver.Filters;
using Newtonsoft.Json;
using System;

namespace FactualDriver.JsonConverters
{
    /// <summary>
    /// Converts Key Value Filter to JSON.
    /// </summary>
    public class KeyValueFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as KeyValueFilter;
            if (filter == null) throw new InvalidOperationException("KeyValueFilterConverter attribute is not applied to KeyValueFilter class");

            serializer.Serialize(writer, filter.Pairs);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}