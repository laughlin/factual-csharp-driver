using FactualDriver.Utils;

namespace FactualDriver
{
    /// <summary>
    /// Represents metadata to be sent with submit and flag requests
    /// </summary>
    public class Metadata
    {
        public const string USER = "user";
        public const string DEBUG = "debug";
        public const string COMMENT = "comment";
        public const string REFERENCE = "reference";

        private Parameters _queryParameters = new Parameters();

        public Metadata() {}

        /// <summary>
        /// Create metadata to associate with submit and flag requests
        /// </summary>
        /// <param name="queryParameters">Query parameters</param>
        public Metadata(Parameters queryParameters)
        {
            _queryParameters = queryParameters;
        }

        /// <summary>
        /// Set a user name for the person submitting the data 
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>new Metadata, with a username set</returns>
        public Metadata User(string username)
        {
            var parameters = _queryParameters.Copy();
            parameters.Add(USER, username);
            return new Metadata(parameters);
        }

        /// <summary>
        /// The request will only be a test query and no actual data will be written
        /// </summary>
        /// <returns>new Metadata, marked as a debug request</returns>
        private Metadata Debug()
        {
            return Debug(true);
        }

        /// <summary>
        /// When true, the request will only be a test query and no actual data will be written.
        /// The default behavior is to NOT include debug.
        /// </summary>
        /// <param name="debug">debug true if you want this to be a test query where no actual date is written</param>
        /// <returns>new Metadata, marked with whether or not this is a debug request</returns>
        private Metadata Debug(bool debug)
        {
            var parameters = _queryParameters.Copy();
            parameters.Add(DEBUG, debug);
            return new Metadata(parameters);
        }

        /// <summary>
        /// Set a comment that will help to explain your corrections
        /// </summary>
        /// <param name="comment">comment the comment that may help explain your corrections</param>
        /// <returns>new Metadata, with a comment set</returns>
        public Metadata Comment(string comment)
        {
            var parameters = _queryParameters.Copy();
            parameters.Add(COMMENT, comment);
            return new Metadata(parameters);
        }

        /// <summary>
        /// Set a reference to a URL, title, person, etc. that is the source of this data
        /// </summary>
        /// <param name="reference">reference a reference to a URL, title, person, etc. that is the source of this data</param>
        /// <returns>new Metadata, with a reference set</returns>
        public Metadata Reference(string reference)
        {
            var parameters = _queryParameters.Copy();
            parameters.Add(REFERENCE, reference);
            return new Metadata(parameters);
        }

        /// <summary>
        /// Converts a path object into a uri path string for communication with
        /// Factual's API. Provides proper URL encoding and escaping.
        /// </summary>
        /// <returns>Returns the path string to represent this Query when talking to Factual's API.</returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(_queryParameters.ToFilterArray());
        }
    }
}