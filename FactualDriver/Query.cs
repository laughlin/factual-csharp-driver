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


        public void Add(IFilter filter)
        {
            if (filter is RowFilter)
            {
                var existingFilter = _filters.FirstOrDefault(p => p.GetType() == typeof(RowFilter));
                if (existingFilter != null)
                {
                    _filters.Remove(existingFilter);
                    _filters.Add(new FilterGroup(new List<IFilter> { existingFilter, filter }));
                    return;
                }

                var existingGroupFilter = _filters.FirstOrDefault(p => p.GetType() == typeof(FilterGroup));
                if (existingGroupFilter != null)
                {
                    ((FilterGroup)existingGroupFilter).RowFilters.Add(filter);
                    return;
                }
            }

            if (filter is FilterGroup)
            {
                var existingGroupFilter = _filters.FirstOrDefault(p => p.GetType() == typeof(FilterGroup));
                if (existingGroupFilter != null)
                {
                    ((FilterGroup)existingGroupFilter).RowFilters.Add(filter);
                    return;
                }
            }

            _filters.Add(filter);

            
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
            SplitGroup((FilterGroup) queries.Last().Filters.FirstOrDefault(p => p.GetType() == typeof (FilterGroup)),
                       queries.Count(), operation);

        }


        public static void SplitGroup(FilterGroup group, int itemsToPopSplit, string newGroupOperator)
        {
            if (group.RowFilters.Count == itemsToPopSplit)
            {
                group.Operator = newGroupOperator;
                return;
            }

            var newGroup =
                new FilterGroup(
                    group.RowFilters.Skip(Math.Max(0, group.RowFilters.Count - itemsToPopSplit)).Take(itemsToPopSplit).
                        ToList(), newGroupOperator);

            group.RowFilters.RemoveRange(group.RowFilters.Count - itemsToPopSplit, itemsToPopSplit);
            group.RowFilters.Add(newGroup);

        }
    }
}