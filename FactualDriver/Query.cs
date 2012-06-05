using System;
using System.Linq;
using System.Collections.Generic;
using FactualDriver.Filters;
using FactualDriver.Utils;

namespace FactualDriver
{
    public class Query : IFilterable
    {
        private List<IFilter> _filters = new List<IFilter>();
        public List<IFilter> Filters { get { return _filters; } set { _filters = value; } } 

        public Query Limit(long limit)
        {
             _filters.Add(new Filter(Constants.QUERY_LIMIT, limit));
            return this;
        }

        public Query Search(string term)
        {
            AddCommaSeparatedFilter(Constants.SEARCH, term);
            return this;
        }

        public Query WithIn(Circle circle)
        {
            _filters.Add(circle.GetFilter());
            return this;
        }

        public Query SortAsc(string field)
        {
            AddCommaSeparatedFilter(Constants.QUERY_SORT, field + ":asc");
            return this;
        }

        public Query SortDesc(string field)
        {
            AddCommaSeparatedFilter(Constants.QUERY_SORT, field + ":desc");
            return this;
        }

        public Query Only(params string[] fields)
        {
            foreach (var field in fields)
            {
                AddCommaSeparatedFilter(Constants.QUERY_SELECT, field);
            }
            return this;
        }

        public Query Offset(int offset)
        {
            AddFilter(Constants.QUERY_OFFSET, offset);
            return this;
        }

        public Query IncludeRowCount(bool includeRowCount)
        {
            AddFilter(Constants.INCLUDE_COUNT, includeRowCount);
            return this;
        }

        public QueryBuilder<Query> Field(string fieldName)
        {
            return new QueryBuilder<Query>(this, fieldName);
        }

        private void AddFilter(string filterName, object value)
        {
            _filters.Add(new Filter(filterName, value));
        }


        private void AddCommaSeparatedFilter(string filterName, string value)
        {
            var filter = _filters.FirstOrDefault(p => p.Name == filterName);
            if(filter != null && filter is Filter)
            {
                var existingFilter = (Filter) filter;
                existingFilter.Value = string.Format("{0},{1}", existingFilter.Value, value);
            }
            else
            {
                _filters.Add(new Filter(filterName,value));
            }
        }


        /// <summary>
        /// Converts a query object into a uri query string for communication with
        /// Factual's API. Provides proper URL encoding and escaping.
        /// </summary>
        /// <returns>Returns the query string to represent this Query when talking to Factual's API.</returns>
        public string ToQueryString()
        {
            return JsonUtil.ToQueryString(_filters.ToArray());
        }


        public void AddRowFilter(IFilter filter)
        {
            if (filter is RowFilter || filter is FilterGroup)
            {
                if(_filters.All(p => p.GetType() != typeof(FilterList)))
                {
                    _filters.Add(new FilterList());
                }

                var list = GetRowFilterList();
                list.Add(filter);
            }
        }

        public List<IFilter> GetRowFilterList()
        {
            return ((FilterList)_filters.Single(p => p.GetType() == typeof(FilterList))).Data;
        }

        public Query And(params Query[] queries)
        {
            PopRowFiltersIntoNewGroup(Constants.FILTER_AND, queries);
            return this;
        }

        public Query Or(params Query[] queries)
        {
            PopRowFiltersIntoNewGroup(Constants.FILTER_OR,queries);
            return this;
        }

        public void PopRowFiltersIntoNewGroup(string operation, Query[] queries)
        {
            var filterCount = queries.Count();
            var filterGroup = new FilterGroup(operation);
            var filterList = GetRowFilterList();
            filterGroup.RowFilters = filterList.Skip(Math.Max(0, filterList.Count - filterCount)).Take(filterCount).ToList();
            filterList.RemoveRange(filterList.Count - filterCount, filterCount);

            AddRowFilter(filterGroup);
        }
    }
}