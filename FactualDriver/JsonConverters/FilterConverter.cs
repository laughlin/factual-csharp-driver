using System;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.JsonConverters
{
    public class FilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as Filter;
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