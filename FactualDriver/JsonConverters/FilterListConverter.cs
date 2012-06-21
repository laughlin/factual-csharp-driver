using System;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.JsonConverters
{
    /// <summary>
    /// Converts FilterList to json.
    /// </summary>
    public class FilterListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filterList = value as FilterList;
            if (filterList == null) throw new InvalidOperationException("FilterListConverter attribute is not applied to FilterList class");

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