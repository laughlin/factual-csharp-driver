using System.Collections.Generic;
using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    [JsonConverter(typeof(ConditionalRowFilterConverter))]
    public class ConditionalRowFilter : IFilter
    {
        #region Implementation of IFilter

        private string _name = "filters";

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        public string Operator { get; set; }
        public IEnumerable<RowFilter> RowFilters { get; set; }

        /// <summary>
        /// Creates a set of filters grouped by condition
        /// </summary>
        /// <param name="conditionalOperator">Condition operator performed on the group</param>
        /// <param name="filters">Collection of filter on which the operator is performed</param>
        public ConditionalRowFilter(string conditionalOperator, IEnumerable<RowFilter> filters)
        {
            Operator = conditionalOperator;
            RowFilters = filters;
        }

    }
}