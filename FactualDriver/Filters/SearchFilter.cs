namespace FactualDriver.Filters
{
    public class SearchFilter : IFilter
    {
        private string _name = Constants.SEARCH;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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