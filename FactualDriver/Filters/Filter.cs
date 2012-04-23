namespace FactualDriver.Filters
{
    public class Filter: IFilter
    {
        #region Implementation of IFilter

        public string ParameterName { get; set; }
        private bool _isText = true;
        public bool IsText
        {
            get { return _isText; }
            set { _isText = value; }
        }

        #endregion

        public string Value { get; set; }

        /// <summary>
        /// Create an instance of a generic filter with key and value
        /// </summary>
        /// <param name="name">Name or key of the filter</param>
        /// <param name="value">Value of the filter</param>
        public Filter(string name, string value)
        {
            ParameterName = name;
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}