using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    /// <summary>
    /// Row filter class for limiting result sets based on individual filter operators. 
    /// </summary>
    [JsonConverter(typeof(RowFilterConverter))]
    public class RowFilter : IFilter
    {
        private string _name = Constants.FILTERS;

        /// <summary>
        /// Name of the field upon which we are going to perform a comparison.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Operator which specifies comparison type.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Value of the filter name to which operator applies.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Short hand version of the row filter which will not set operator
        /// </summary>
        /// <param name="fieldName">Field name on which filter is performed</param>
        /// <param name="compareToValue">Filter value</param>
        public RowFilter(string fieldName, object compareToValue)
        {
            FieldName = fieldName;
            Value = compareToValue;
        }

        /// <summary>
        /// Row filter wrapper which will get serialized into proper json object filter when calling factual api
        /// </summary>
        /// <param name="fieldName">Field name on which this filter is performed</param>
        /// <param name="compareOperator">Comparison operator key</param>
        /// <param name="compareValue">Value</param>
        public RowFilter(string fieldName, string compareOperator, object compareValue)
        {
            FieldName = fieldName;
            Operator = compareOperator;
            Value = compareValue;
        }

        #region Implementation of IFilter

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
    }
}