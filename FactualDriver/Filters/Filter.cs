using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    [JsonConverter(typeof(FilterConverter))]
    public class Filter: IFilter
    {
        #region Implementation of IFilter

        public string Name { get; set; }

        #endregion

        public object Value { get; set; }

        /// <summary>
        /// Create an instance of a generic filter with key and value
        /// </summary>
        /// <param name="name">Name or key of the filter</param>
        /// <param name="value">Value of the filter</param>
        public Filter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}