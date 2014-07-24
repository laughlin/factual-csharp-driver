namespace FactualDriver
{
    /// <summary>
    /// Represents a Factual Diffs query.
    /// </summary>
    public class DiffsQuery
    {
        /// <summary>
        /// Holds all parameters for this DiffsQuery.
        /// </summary>
        private Parameters _queryParameters = new Parameters();
        
        /// <summary>
        /// Constructor. Create a request to find diffs on a Factual table between two times.
        /// </summary>
        /// <returns>DiffsQuery</returns>
        public DiffsQuery()
        {
            
        }

        /// <summary>
        /// Constructor. Create a request to find diffs on a Factual table between two times.
        /// </summary>
        /// <param name="after">begin time to create this diff against.</param>
        /// <returns>DiffsQuery</returns>
        public DiffsQuery(long after)
        {
            After(after);
        }

        private bool isValidTimestamp(long timestamp)
        {
            return (timestamp > 1325376000000);
        }

        /// <summary>
        /// Begin time to create this diff against.
        /// </summary>
        /// <param name="timestamp">timestamp begin time for this diff.</param>
        /// <returns>DiffsQuery</returns>
        public DiffsQuery After(long timestamp)
        {
            //if(isValidTimestamp(timestamp))
                AddParam(Constants.DIFFS_START_DATE, timestamp);
            return this;
        }

        /// <summary>
        /// End time to create this diff against.
        /// </summary>
        /// <param name="timestamp">timestamp end time for this diff.</param>
        /// <returns>DiffsQuery</returns>
        public DiffsQuery Before(long timestamp)
        {
            //if(isValidTimestamp(timestamp))
                AddParam(Constants.DIFFS_END_DATE, timestamp);
            return this;
        }

        /// <summary>
        /// Set a parameter and value pair for specifying url parameters, specifically those not yet available as convenience methods.
        /// </summary>
        /// <param name="key"> the field name of the parameter to add</param>
        /// <param name="value">the field value that will be serialized using value.toString()</param>
        private DiffsQuery AddParam(string key, object value)
        {
            _queryParameters.Add(key,value);
            return this;
        }

        /// <summary>
        /// Converts DiffQuery object into url encoded string
        /// </summary>
        /// <returns>string</returns>
        public string ToUrlQuery()
        {
            return _queryParameters.ToUrlQuery();
        }
    }
}