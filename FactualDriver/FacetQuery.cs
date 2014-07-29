using FactualDriver.Filters;
using FactualDriver.Utils;
using System;
using System.Collections.Generic;

namespace FactualDriver
{
    /// <summary>
    /// Represents a top level Factual facet query. Knows how to represent the facet
    /// query as URL encoded key value pairs, ready for the query string in a GET
    /// request.
    /// </summary>
    public class FacetQuery : IQuery
    {
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fields">fields for which facets will be generated</param>
        public FacetQuery(params string[] fields) : this((IEnumerable<String>)fields) { }

        public FacetQuery(IEnumerable<String> fields)
        {
            Select(fields);
        }

        /// <summary>
        /// The fields for which facets should be generated. The response will not be ordered identically to this list, nor will it reflect any nested relationships between fields.
        /// </summary>
        /// <param name="fields">fields the fields for which facets should be generated. The response will not be ordered identically to this list, nor will it reflect any nested relationships between fields.</param>
        private void Select(params string[] fields)
        {
            Select((IEnumerable<String>)fields);
        }

        /// <summary>
        /// The fields for which facets should be generated. The response will not be ordered identically to this list, nor will it reflect any nested relationships between fields.
        /// </summary>
        /// <param name="fields">fields the fields for which facets should be generated. The response will not be ordered identically to this list, nor will it reflect any nested relationships between fields.</param>
        private void Select(IEnumerable<String> fields)
        {
            foreach (var field in fields)
            {
                _parameters.AddCommaSeparatedFilter(Constants.FACET_SELECT, field);
            }
        }

        /// <summary>
        /// Sets a full text search query. Factual will use this value to perform a
        /// full text search against various attributes of the underlying table, such
        /// as entity name, address, etc.
        /// </summary>
        /// <param name="term">the text for which to perform a full text search.</param>
        /// <returns>FacetQuery</returns>
        public FacetQuery Search(string term)
        {
            AddFilter(new Filter(Constants.SEARCH, term));
            return this;
        }

        /// <summary>
        /// Add query to this filter.
        /// </summary>
        /// <param name="filter"></param>
        [Obsolete("please use AddFilter() instead")]
        public void Add(IFilter filter)
        {
            AddFilter(filter);
        }

        public FacetQuery AddFilter(IFilter filter)
        {
            _parameters.Add(filter);
            return this;
        }

        /// <summary>
        /// Convert this Query object to url encoded query string.
        /// </summary>
        /// <returns></returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_parameters.ToFilterArray());
        }

        /// <summary>
        /// Begins construction of a new row filter for this FacetQuery
        /// </summary>
        /// <param name="field">the name of the field on which to filter.</param>
        /// <returns>A partial representation of the new row filter.</returns>
        public QueryBuilder<FacetQuery> Field(string field)
        {
            return new QueryBuilder<FacetQuery>(this, field);
        }

        /// <summary>
        /// Adds a filter so that results can only be (roughly) within the specified
        /// geographic circle.
        /// </summary>
        /// <param name="circle">The circle within which to bound the results.</param>
        /// <returns>FacetQuery</returns>
        public FacetQuery Within(Circle circle)
        {
            AddFilter(circle.GetFilter());
            return this;
        }

        /// <summary>
        /// Used to nest AND'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public FacetQuery And(params FacetQuery[] queries)
        {
            return And((IEnumerable<FacetQuery>)queries);
        }

        /// <summary>
        /// Used to nest AND'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public FacetQuery And(IEnumerable<FacetQuery> queries)
        {
            _parameters.PopRowFiltersIntoNewGroup(Constants.FILTER_AND, queries);
            return this;
        }

        /// <summary>
        /// Used to nest OR'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public FacetQuery Or(params FacetQuery[] queries)
        {
            return Or((IEnumerable<FacetQuery>)queries);
        }

        /// <summary>
        /// Used to nest OR'ed predicates.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public FacetQuery Or(IEnumerable<FacetQuery> queries)
        {
            _parameters.PopRowFiltersIntoNewGroup(Constants.FILTER_OR, queries);
            return this;
        }

        /// <summary>
        /// The response will include a count of the total number of rows in the
        /// table that conform to the request based on included filters. This will
        /// increase the time required to return a response. The default behavior is
        /// to NOT include a row count
        /// </summary>
        /// <returns></returns>
        public FacetQuery IncludeRowCount()
        {
            return IncludeRowCount(true);
        }

        /// <summary>
        /// When true, the response will include a count of the total number of rows
        /// in the table that conform to the request based on included filters.
        /// Requesting the row count will increase the time required to return a
        /// response. The default behavior is to NOT include a row count.
        /// </summary>
        /// <param name="includeRowCount">
        /// true if you want the results to include a count of the total
        /// number of rows in the table that conform to the request based
        /// on included filters
        /// </param>
        /// <returns>FacetQuery</returns>
        public FacetQuery IncludeRowCount(bool includeRowCount)
        {
            _parameters.Add(Constants.INCLUDE_COUNT, includeRowCount);
            return this;
        }

        /// <summary>
        /// For each facet value count, the minimum number of results it must have in order to be returned in the response. Must be zero or greater. The default is 1.
        /// </summary>
        /// <param name="minCount">minCount for each facet value count, the minimum number of results it must have in order to be returned in the response. Must be zero or greater. The default is 1.</param>
        /// <returns>FacetQuery</returns>
        public FacetQuery MinCountPerFacetValue(long minCount)
        {
            AddFilter(new Filter(Constants.FACET_MIN_COUNT_PER_FACET_VALUE, minCount));
            return this;
        }

        /// <summary>
        /// The maximum number of unique facet values that can be returned for a single field. Range is 1-250. The default is 25.
        /// </summary>
        /// <param name="maxValuesPerFacet">maxValuesPerFacet the maximum number of unique facet values that can be returned for a single field. Range is 1-250. The default is 25.</param>
        /// <returns>FacetQuery</returns>
        public FacetQuery MaxValuesPerFacet(long maxValuesPerFacet)
        {
            AddFilter(new Filter(Constants.FACET_MAX_VALUES_PER_FACET, maxValuesPerFacet));
            return this;
        }

       


    }
}