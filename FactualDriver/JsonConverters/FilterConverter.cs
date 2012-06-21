using System;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.JsonConverters
{
    /// <summary>
    /// Converts Filter to json.
    /// </summary>
    public class FilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as Filter;
            if(filter == null) throw new InvalidOperationException("FilterConverter attribute is not applied to Filter class");

            if(filter.Value is bool) //lowercase boolean values
            {
                writer.WriteRaw(filter.Value.ToString().ToLower());
            }
            else
            {
                writer.WriteRaw(filter.Value.ToString());
            }
            
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