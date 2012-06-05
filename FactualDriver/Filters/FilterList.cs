using System.Collections.Generic;
using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    [JsonConverter(typeof(FilterListConverter))]
    public class FilterList : IFilter
    {
        private string _name = Constants.FILTERS;
        public string Name { get { return _name; } set { _name = value; } }
        public List<IFilter> Data { get; set; }

        public FilterList()
        {
            Data = new List<IFilter>();
        }
    }
}