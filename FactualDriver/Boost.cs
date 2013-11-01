using FactualDriver.Filters;
using FactualDriver.Utils;

namespace FactualDriver
{
    public class Boost
    {
        /// <summary>
        /// Represents a top level Factual query. Knows how to represent the query as URL
        /// encoded key value pairs, ready for the query string in a GET request.
        /// </summary>
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Constructor for a boost.
        /// </summary>
        /// <param name="factualId">factualId this boost is performed on.</param>
        public Boost(string factualId)
        {
          Add(new Filter(Constants.BOOST_FACTUAL_ID, factualId));
        }

        /// <summary>
        /// Constructor for a boost.
        /// </summary>
        /// <param name="factualId">factualId this boost is performed on.</param>
        /// <param name="query">factualId this boost is performed on.</param>
        /// <param name="user">factualId this boost is performed on.</param>
        public Boost(string factualId, Query query, Metadata user)
            : this(factualId)
        {
            string[] querySearchArray = query.ToUrlQuery().Split('=', '&');
            for (int i = 0; i < querySearchArray.Length; i++)
              if (querySearchArray[i] == Constants.SEARCH && i + 1 < querySearchArray.Length && !string.IsNullOrWhiteSpace(querySearchArray[i + 1]))
                Search(querySearchArray[i + 1]);

            string[] queryUserArray = user.ToUrlQuery().Split('=', '&');
            for (int i = 0; i < queryUserArray.Length; i++)
              if (queryUserArray[i] == Constants.USER && i + 1 < queryUserArray.Length && !string.IsNullOrWhiteSpace(queryUserArray[i + 1]))
                User(queryUserArray[i + 1]);
        }

        /// <summary>
        /// Sets a full text search for this boost.
        /// </summary>
        /// <param name="term">the text for which to perform this boost.</param>
        /// <returns>this Boost</returns>
        public Boost Search(string term)
        {
            Add(new Filter(Constants.SEARCH, term));
            return this;
        }

        /// <summary>
        /// Sets a user for this boost.
        /// </summary>
        /// <param name="user">the user who performs this boost.</param>
        /// <returns>this Boost</returns>
        public Boost User(string user)
        {
            Add(new Filter(Constants.USER, user));
            return this;
        }

        /// <summary>
        /// Adds filter to this Boost.
        /// </summary>
        /// <param name="filter"></param>
        private Boost Add(IFilter filter)
        {
            _parameters.Add(filter);
            return this;
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
    }
}
