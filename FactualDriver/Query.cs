using System.Linq;
using System.Collections.Generic;
using FactualDriver.Filters;
using FactualDriver.Utils;

namespace FactualDriver
{
    public class Query
    {
        private readonly List<IFilter> _filters = new List<IFilter>();

        public Query Limit(long limit)
        {
             _filters.Add(new Filter(Constants.QUERY_LIMIT, limit));
            return this;
        }

        public Query Search(string term)
        {
            _filters.Add(new Filter(Constants.SEARCH, term));
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
        /// Builds and returns the query string to represent this Query when talking to
        /// Factual's API. Provides proper URL encoding and escaping.
        /// Example output:
        /// filters=%7B%22%24and%22%3A%5B%7B%22region%22%3A%7B%22%24in%22%3A%5B%22MA%22%2C%22VT%22%2C%22NH%22%5D%7D%7D%2C%7B%22%24or%22%3A%5B%7B%22first_name%22%3A%7B%22%24eq%22%3A%22Chun%22%7D%7D%2C%7B%22last_name%22%3A%7B%22%24eq%22%3A%22Kok%22%7D%7D%5D%7D%5D%7D
        /// (After decoding, the above example would be used by the server as:)
        /// filters={"$and":[{"region":{"$in":["MA","VT","NH"]}},{"$or":[{"first_name":{"$eq":"Chun"}},{"last_name":{"$eq":"Kok"}}]}]}
        /// </summary>
        /// <returns>Returns the query string to represent this Query when talking to Factual's API.</returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_filters.ToArray());
        }
    }
}