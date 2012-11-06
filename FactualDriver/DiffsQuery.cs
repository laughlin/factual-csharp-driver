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
        /// Constructor.  Create a request to find diffs on a Factual table between two times.
        /// </summary>
        /// <param name="before">before the before time to create this diff against.</param>
        public DiffsQuery(long before)
        {
            Before(before);
        }

        /// <summary>
        /// The before time to create this diff against.
        /// </summary>
        /// <param name="timestamp">timestamp before time for this diff.</param>
        /// <returns>DiffsQuery</returns>
        private DiffsQuery Before(long timestamp)
        {
            AddParam(Constants.DIFFS_START_DATE, timestamp);
            return this;
        }

        /// <summary>
        /// The after time to create this diff against.
        /// </summary>
        /// <param name="timestamp">timestamp after time for this diff.</param>
        /// <returns>this DiffsQuery</returns>
        public DiffsQuery After(long timestamp)
        {
            AddParam(Constants.DIFFS_END_DATE, timestamp);
            return this;
        }

        /// <summary>
        /// Set a parameter and value pair for specifying url parameters, specifically those not yet available as convenience methods.
        /// </summary>
        /// <param name="key"> the field name of the parameter to add</param>
        /// <param name="value">the field value that will be serialized using value.toString()</param>
        private void AddParam(string key, object value)
        {
            _queryParameters.Add(key,value);
        }

        public string ToUrlQuery()
        {
            return _queryParameters.ToUrlQuery();
        }
    }
}