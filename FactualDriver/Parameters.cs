using System;
using System.Collections.Generic;
using System.Linq;
using FactualDriver.Filters;
using FactualDriver.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FactualDriver
{
    /// <summary>
    /// A class holder of multiple filters, responsible for logic associated with filters collections.
    /// </summary>
    public class Parameters
    {
        private List<IFilter> _filters = new List<IFilter>();

        public Parameters() {}

        public Parameters(List<IFilter> filters)
        {
            _filters = filters;
        }

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
        /// Adds a json filter parameter.
        /// </summary>
        /// <param name="name">Name of the filter</param>
        /// <param name="keyValuePairs">key value pairs to be serialized</param>
        public void AddJsonFilter(string name, Dictionary<string,object> keyValuePairs)
        {
            _filters.Add(new Filter(name, keyValuePairs));
        }

        /// <summary>
        /// Adds a pair of values to the specified Key Value Filter.
        /// </summary> 
        /// <param name="filterName">Filter name to add a pair</param>
        /// <param name="field">Field or a key in the pair</param>
        /// <param name="value">Value of the pair</param>
        public void SetJsonPair(string filterName, string field, object value)
        {
            var filter = _filters.SingleOrDefault(p => p.Name == filterName);
            if(filter != null && filter is KeyValueFilter)
            {
                var existingFilter = (KeyValueFilter)filter;
                existingFilter.Pairs.Add(field, value);
            }
            else
            {
                _filters.Add(new KeyValueFilter(filterName, new Dictionary<string, object> {{field, value}}));
            }
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

            if (filterCount > filterList.Count)
                filterCount = filterList.Count();

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

        public Parameters Copy()
        {
            return new Parameters(new List<IFilter>(_filters));
        }

        /// <summary>
        /// Outputs parameters into url encoded query string.
        /// </summary>
        /// <returns>Url Encoded Query string</returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_filters.ToArray());
        }
    }
}