namespace FactualDriver.Filters
{
    public class SearchFilter : IFilter
    {
        private string _parameterName = "q";
        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        private bool _isText = true;
        public bool IsText
        {
            get { return _isText; }
            set { _isText = value; }
        }

        public string SearchText { get; set; }

        /// <summary>
        /// Search filter to do a full-text search on factual api
        /// </summary>
        /// <param name="searchText">Test to search</param>
        public SearchFilter(string searchText)
        {
            SearchText = searchText;
        }

        public override string ToString()
        {
            return SearchText;
        }
    }
}