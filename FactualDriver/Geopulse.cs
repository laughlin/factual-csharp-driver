using FactualDriver.Filters;
using FactualDriver.Utils;
using Newtonsoft.Json;

namespace FactualDriver
{
    /// <summary>
    /// Represents a Geopulse query against Factual
    /// </summary>
    public class Geopulse
    {
        /// <summary>
        /// Holds all parameters for this Geopulse
        /// </summary>
        private Parameters _parameters = new Parameters();

        public Geopulse(Point point)
        {
            _parameters.Add(point);
        }

        /// <summary>
        /// Sets the fields to select. This is optional; default behaviour is generally
        /// to select all fields in the schema.
        /// </summary>
        /// <param name="fields">fields to select</param>
        /// <returns>Geopulse</returns>
        public Geopulse Only(params string[] fields)
        {
            foreach (var field in fields)
            {
                _parameters.AddCommaSeparatedFilter(Constants.QUERY_SELECT, field);
            }
            return this;
        }

        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_parameters.ToFilterArray());
        }
    }
}