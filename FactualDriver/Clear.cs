namespace FactualDriver
{
    /// <summary>
    /// Represents a clear request of existing attributes on a Factual entity.
    /// </summary>
    public class Clear
    {
        /// <summary>
        /// Holds all parameters for this Clear.
        /// </summary>
        private Parameters _parameters = new Parameters();

        public Clear() { }

        /// <summary>
        /// Constructor for a clear with fields passed as unknown number of strings.
        /// </summary>
        /// <param name="fields">fields this clear is initialized with</param>
        public Clear(params string[] fields)
        {
           foreach(var field in fields)
                _parameters.AddCommaSeparatedFilter(Constants.CLEAR_FIELDS, field);
        }

        /// <summary>
        ///Add a single field for this clear request.
        /// </summary>
        /// <param name="field">the field name</param>
        /// <returns>this Clear</returns>
        public Clear AddField(string field)
        {
            _parameters.AddCommaSeparatedFilter(Constants.CLEAR_FIELDS, field);
            return this;
        }

        /// <summary>
        /// Output clear values to url query
        /// </summary>
        /// <returns>Url encoded query string parameters</returns>
        public string ToUrlQuery()
        {
            return _parameters.ToUrlQuery();
        }
    }
}