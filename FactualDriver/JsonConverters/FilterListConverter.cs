using System;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.JsonConverters
{
    public class FilterListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filterList = value as FilterList;
            if(filterList.Data.Count == 1)
            {
                writer.WriteRaw(JsonConvert.SerializeObject(filterList.Data[0]));
            }
            else
            {
                writer.WriteRaw(JsonConvert.SerializeObject(new FilterGroup(filterList.Data)));
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