using System;
using System.Linq;
using System.Collections.Generic;
using FactualDriver.Filters;
using FactualDriver.Utils;

namespace FactualDriver
{
    public class Query : IQuery
    {
        private Parameters _parameters = new Parameters();

        public Query Limit(long limit)
        {
            Add(new Filter(Constants.QUERY_LIMIT, limit));
            return this;
        }

        public Query Search(string term)
        {
            _parameters.AddCommaSeparatedFilter(Constants.SEARCH, term);
            return this;
        }

        public Query WithIn(Circle circle)
        {
            Add(circle.GetFilter());
            return this;
        }

        public Query SortAsc(string field)
        {
            _parameters.AddCommaSeparatedFilter(Constants.QUERY_SORT, field + ":asc");
            return this;
        }

        public Query SortDesc(string field)
        {
            _parameters.AddCommaSeparatedFilter(Constants.QUERY_SORT, field + ":desc");
            return this;
        }

        public Query Only(params string[] fields)
        {
            foreach (var field in fields)
            {
                _parameters.AddCommaSeparatedFilter(Constants.QUERY_SELECT, field);
            }
            return this;
        }

        public Query Offset(int offset)
        {
            _parameters.Add(Constants.QUERY_OFFSET, offset);
            return this;
        }

        public Query IncludeRowCount(bool includeRowCount)
        {
            _parameters.Add(Constants.INCLUDE_COUNT, includeRowCount);
            return this;
        }

        public QueryBuilder<Query> Field(string fieldName)
        {
            return new QueryBuilder<Query>(this, fieldName);
        }

        /// <summary>
        /// Converts a path object into a uri path string for communication with
        /// Factual's API. Provides proper URL encoding and escaping.
        /// </summary>
        /// <returns>Returns the path string to represent this Query when talking to Factual's API.</returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_parameters.ToFilterArray());
        }

        public void Add(IFilter filter)
        {
            _parameters.Add(filter);
        }

        /// <summary>
        /// Used to nest AND'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public Query And(params Query[] queries)
        {
            _parameters.PopRowFiltersIntoNewGroup(Constants.FILTER_AND, queries);
            return this;
        }

        /// <summary>
        /// Used to nest OR'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public Query Or(params Query[] queries)
        {
            _parameters.PopRowFiltersIntoNewGroup(Constants.FILTER_OR, queries);
            return this;
        }
    }
}