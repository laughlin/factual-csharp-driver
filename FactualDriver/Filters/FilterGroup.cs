using System.Collections.Generic;
using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    [JsonConverter(typeof(FilterGroupConverter))]
    public class FilterGroup : IFilter
    {
        #region Implementation of IFilter

        private string _name = Constants.FILTERS;


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        public string Operator { get; set; }
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

        public FilterGroup(string conditionalOperator)
        {
            Operator = conditionalOperator;
            RowFilters = new List<IFilter>();
        }

    }
}