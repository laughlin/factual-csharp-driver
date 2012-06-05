using System.Collections.Generic;
using FactualDriver.Filters;
using FactualDriver.Utils;

namespace FactualDriver
{
    public class CrosswalkQuery : IQuery
    {
        /// <summary>
        /// Holds all parameters for this CrosswalkQuery.
        /// </summary>
        private Parameters _parameters = new Parameters();

        public void Add(IFilter filter)
        {
            _parameters.Add(filter);
        }

        /// <summary>
        /// Adds the specified Factual ID to this Query. Returned Crosswalk data will
        /// be for only the entity associated with the Factual ID.
        /// </summary>
        /// <param name="id">a unique Factual ID</param>
        /// <returns>CrosswalkQuery</returns>
        public CrosswalkQuery FactualId(string id)
        {
            Add(new Filter(Constants.CROSSWALK_FACTUAL_ID, id));
            return this;
        }

        /// <summary>
        /// Adds the specified limit to this Query. The amount of returned
        /// Crosswalk records will not exceed this limit.
        /// </summary>
        /// <param name="limit">The amount of returned Crosswalk records this Query will not exceed.</param>
        /// <returns>CrosswalkQuery</returns>
        public CrosswalkQuery Limit(int limit)
        {
            Add(new Filter(Constants.CROSSWALK_LIMIT,limit));
            return this;
        }

        /// <summary>
        /// The namespace to search for a third party ID within.
        /// </summary>
        /// <param name="namespaceName">The namespace to search for a third party ID within.</param>
        /// <returns>CrosswalkQuery</returns>
        public CrosswalkQuery Namespace(string namespaceName)
        {
            Add(new Filter(Constants.CROSSWALK_NAMESPACE, namespaceName));
            return this;
        }

        /// <summary>
        /// The id used by a third party to identify a place.
        /// </summary>
        /// <param name="namespaceId">The id used by a third party to identify a place.</param>
        /// <returns>CrosswalkQuery</returns>
        public CrosswalkQuery NamespaceId(string namespaceId)
        {
            Add(new Filter(Constants.CROSSWALK_NAMESPACE_ID, namespaceId));
            return this;
        }

        /// <summary>
        /// Restricts the results to only return ids for the specified namespace(s).
        /// </summary>
        /// <param name="namespaces">parameters namespaces</param>
        /// <returns>CrosswalkQuery</returns>
        public CrosswalkQuery Only(params string[] namespaces)
        {
            foreach (var name in namespaces)
            {
                _parameters.AddCommaSeparatedFilter(Constants.CROSSWALK_ONLY, name);
            }
            return this;
        }

        /// <summary>
        /// Converts a query object into a uri query string for communication with
        /// Factual's API. Provides proper URL encoding and escaping.
        /// </summary>
        /// <returns>Returns the query string to represent this Query when talking to Factual's API.</returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_parameters.ToFilterArray());
        }
    }
}