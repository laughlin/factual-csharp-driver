using System.Linq;
using System.Collections.Generic;

namespace FactualDriver
{
    /// <summary>
    /// Represents an add or update submission to a Factual row.
    /// </summary>
    public class Submit
    {
        /// <summary>
        /// Holds all parameters for this Submit.
        /// </summary>
        private Parameters _parameters = new Parameters();

        public Submit() {}

        /// <summary>
        /// Constructor for a submit with values initialized as key value pairs in mapping.
        /// </summary>
        /// <param name="values">values this submit is initialized with</param>
        public Submit(Dictionary<string,object> values)
        {
            _parameters.AddJsonFilter(Constants.SUBMIT_VALUES, values);
        }

        /// <summary>
        /// Set the value for a single field in this submit request.
        /// Added to a JSON hash of field names and values to be added to a Factual table.
        /// </summary>
        /// <param name="field">the field name</param>
        /// <param name="value">the value for the specified field</param>
        /// <returns>this Submit</returns>
        public Submit AddValue(string field, object value)
        {
            _parameters.SetJsonPair(Constants.SUBMIT_VALUES, field, value);
            return this;
        }

        /// <summary>
        /// Set the value to null for a single field in this submit request.
        /// </summary>
        /// <param name="field">field the field to set as empty, or null.</param>
        /// <returns>this Submit</returns>
        public Submit RemoveValue(string field)
        {
            AddValue(field, null);
            return this;
        }

        /// <summary>
        /// Output submit values to url query
        /// </summary>
        /// <returns>Url encoded query string parameters</returns>
        public string ToUrlQuery()
        {
            return _parameters.ToUrlQuery();
        }
    }
}