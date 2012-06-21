using System.Collections.Generic;
using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    /// <summary>
    /// FilterList is a helper filter that has a list of filters, if there is only one filter it
    /// will get serialized as a value and not an array. And if there are multiple filters
    /// it will get serialized as a filter group array with $and operator.
    /// </summary>
    [JsonConverter(typeof(FilterListConverter))]
    public class FilterList : IFilter
    {
        private string _name = Constants.FILTERS;
        /// <summary>
        /// Key of the filter.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        /// <summary>
        /// List of filters
        /// </summary>
        public List<IFilter> Data { get; set; }

        /// <summary>
        /// Parameterless constructor which creates an empty array of filters.
        /// </summary>
        public FilterList()
        {
            Data = new List<IFilter>();
        }
    }
}