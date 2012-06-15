using System;
using System.Collections.Generic;
using System.Linq;
using FactualDriver.Filters;

namespace FactualDriver
{
    /// <summary>
    /// A class holder of multiple filters, responsible for logic associated with filters collections.
    /// </summary>
    public class Parameters
    {
        private List<IFilter> _filters = new List<IFilter>();
        public List<IFilter> Filters { get { return _filters; } set { _filters = value; } }

        /// <summary>
        /// Adds filter to collection
        /// </summary>
        /// <param name="filterName">Name of the filter</param>
        /// <param name="value">Filter's value</param>
        public void Add(string filterName, object value)
        {
            _filters.Add(new Filter(filterName, value));
        }

        /// <summary>
        /// Adds filter to collection
        /// </summary>
        /// <param name="filter">Filter object</param>
        public void Add(IFilter filter)
        {
            if (filter is RowFilter || filter is FilterGroup)
            {
                AddRowFilter(filter);
                return;
            }

            _filters.Add(filter);
        }

        private void AddRowFilter(IFilter filter)
        {
            if (_filters.All(p => p.GetType() != typeof(FilterList)))
            {
                _filters.Add(new FilterList());
            }

            var list = GetRowFilterList();
            list.Add(filter);
        }

        /// <summary>
        /// Adds a filter, if filter already exists then it would comma separate values
        /// </summary>
        /// <param name="filterName">Filter's name</param>
        /// <param name="value">value to add or comma separate</param>
        public void AddCommaSeparatedFilter(string filterName, string value)
        {
            var filter = _filters.FirstOrDefault(p => p.Name == filterName);
            if (filter != null && filter is Filter)
            {
                var existingFilter = (Filter)filter;
                existingFilter.Value = string.Format("{0},{1}", existingFilter.Value, value);
            }
            else
            {
                _filters.Add(new Filter(filterName, value));
            }
        }

        /// <summary>
        /// Returns only row filters from filter collection.
        /// </summary>
        /// <returns>Collection of filters</returns>
        public List<IFilter> GetRowFilterList()
        {
            return ((FilterList)_filters.Single(p => p.GetType() == typeof(FilterList))).Data;
        }

        /// <summary>
        /// Groups filters recently added to collection by operation, used with conditional operators.
        /// </summary>
        /// <param name="operation">Conditional operations</param>
        /// <param name="queries">Queries to group</param>
        public void PopRowFiltersIntoNewGroup(string operation, IQuery[] queries)
        {
            var filterCount = queries.Count();
            var filterGroup = new FilterGroup(operation);
            var filterList = GetRowFilterList();
            filterGroup.RowFilters = filterList.Skip(Math.Max(0, filterList.Count - filterCount)).Take(filterCount).ToList();
            filterList.RemoveRange(filterList.Count - filterCount, filterCount);

            Add(filterGroup);
        }

        /// <summary>
        /// Converts collections to filter array.
        /// </summary>
        /// <returns>IFilter array</returns>
        public IFilter[] ToFilterArray()
        {
            return _filters.ToArray();
        }
    }
}