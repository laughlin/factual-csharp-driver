using System.Linq;
using System.Text;
using FactualDriver.Filters;
using FactualDriver.Utils;

namespace FactualDriver
{
    /// <summary>
    /// Represents a Factual row query. Knows how to represent the query as URL
    /// encoded key value pairs, ready for the query string in a GET request.
    /// </summary>
    public class RowQuery
    {
        /// <summary>
        /// Holds all parameters for this query
        /// </summary>
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Sets the fields to select. This is optional; default behaviour is generally
        /// to select all fields in the schema.
        /// </summary>
        /// <param name="fields">the fields to select.</param>
        /// <returns>this Query</returns>
        public RowQuery Only(params string[] fields)
        {
            foreach (var field in fields)
            {
                _parameters.AddCommaSeparatedFilter(Constants.QUERY_SELECT, field);
            }
            return this;
        }

        /// <summary>
        /// Converts a path object into a uri path string for communication with
        /// Factual's API. Provides proper URL encoding and escaping.
        /// </summary>
        /// <returns>Returns the path string to represent this RowQuery when talking to Factual's API.</returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_parameters.ToFilterArray());
        }
    }
}