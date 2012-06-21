using System.Collections.Generic;
using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    /// <summary>
    /// Class represents a collection of row filters grouped by conditional operators.
    /// </summary>
    [JsonConverter(typeof(FilterGroupConverter))]
    public class FilterGroup : IFilter
    {
        #region Implementation of IFilter

        private string _name = Constants.FILTERS;

        /// <summary>
        /// Key of the filter. Defaults to the correct Factual Filters keys.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
        /// <summary>
        /// Filter conditional operator.
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// A collection of row filters under FilterGroup.
        /// </summary>
        public List<IFilter> RowFilters { get; set; }

        /// <summary>
        /// Creates a set of filters grouped by condition
        /// </summary>
        /// <param name="conditionalOperator">Condition operator performed on the group</param>
        /// <param name="filters">Collection of filter on which the operator is performed</param>
        public FilterGroup(List<IFilter> filters, string conditionalOperator = Constants.FILTER_AND)
        {
            Operator = conditionalOperator;
            RowFilters = filters;
        }

        /// <summary>
        /// Creates an empty Filters group with specified conditional operator.
        /// </summary>
        /// <param name="conditionalOperator">Operator</param>
        public FilterGroup(string conditionalOperator)
        {
            Operator = conditionalOperator;
            RowFilters = new List<IFilter>();
        }

    }
}