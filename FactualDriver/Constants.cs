namespace FactualDriver
{
    /// <summary>
    /// Helper class that keeps all factual related constant strings.
    /// </summary>
    public class Constants
    {
        // Circle
	    public const string CIRCLE = "$circle";
	    public const string CENTER = "$center";
	    public const string METERS = "$meters";

        // PointKey
        public const string POINT = "$point";

	    // Response
	    public const string RESPONSE = "response";
	    public const string TOTAL_ROW_COUNT = "total_row_count";
	    public const string INCLUDED_ROWS = "included_rows";
	    public const string STATUS = "status";
	    public const string VERSION = "version";

	    // Schema
	    public const string SCHEMA_COLUMN_NAME = "name";
	    public const string SCHEMA_COLUMN_DESCRIPTION = "description";
	    public const string SCHEMA_COLUMN_LABEL = "label";
	    public const string SCHEMA_COLUMN_DATATYPE = "datatype";
	    public const string SCHEMA_COLUMN_FACETED = "faceted";
	    public const string SCHEMA_COLUMN_SORTABLE = "sortable";
	    public const string SCHEMA_COLUMN_SEARCHABLE = "searchable";
	    public const string SCHEMA_VIEW = "view";
	    public const string SCHEMA_FIELDS = "fields";
	    public const string SCHEMA_TITLE = "title";
	    public const string SCHEMA_DESCRIPTION = "description";
	    public const string SCHEMA_SEARCH_ENABLED = "search_enabled";
	    public const string SCHEMA_GEO_ENABLED = "geo_enabled";

	    // Crosswalk
	    public const string CROSSWALK_FACTUAL_ID = "factual_id";
	    public const string CROSSWALK_LIMIT = "limit";
	    public const string CROSSWALK_NAMESPACE = "namespace";
	    public const string CROSSWALK_NAMESPACE_ID = "namespace_id";
	    public const string CROSSWALK_ONLY = "only";

	    public const string CROSSWALK_DATA = "data";
	    public const string CROSSWALK_URL = "url";

        // Diffs

        public const string DIFFS_START_DATE = "start-date";
        public const string DIFFS_END_DATE = "end-date";
        public const string DIFFS_DATA = "data";

	    // Filters
	    public const string FILTERS = "filters";
	    public const string FILTER_GEO = "geo";
	    public const string FILTER_AND = "$and";
	    public const string FILTER_OR = "$or";

	    // Common query
	    public const string INCLUDE_COUNT = "include_count";
	    public const string SEARCH = "q";
	
	    // Query
	    public const string QUERY_LIMIT = "limit";
	    public const string QUERY_OFFSET = "offset";
	    public const string QUERY_SORT = "sort";
	    public const string QUERY_SELECT = "select";
	
	    public const string QUERY_DATA = "data";
	
	    // Facet
	    public const string FACET_MIN_COUNT_PER_FACET_VALUE = "min_count";
	    public const string FACET_MAX_VALUES_PER_FACET = "limit";
	    public const string FACET_SELECT = "select";

	    public const string FACET_DATA = "data";
	
	    // Resolve
	    public const string RESOLVE_VALUES = "values";

        	// Submit
	    public const string SUBMIT_VALUES = "values";
	    public const string SUBMIT_FACTUAL_ID = "factual_id";
	    public const string SUBMIT_NEW_ENTITY = "new_entity";

	    // Contribute
	    public const string CONTRIBUTE_VALUES = "values";
	    public const string CONTRIBUTE_FACTUAL_ID = "factual_id";
	    public const string CONTRIBUTE_NEW_ENTITY = "new_entity";

        //Operators
        public const string OPERATOR_EQUAL = "$eq";
        public const string OPERATOR_NOT_EQUAL = "$neq";
        public const string OPERATOR_EQUALS_ANY = "$in";
        public const string OPERATOR_NOT_EQUALS_ANY = "$nin";
        public const string OPERATOR_BEGINS_WITH = "$bw";
        public const string OPERATOR_NOT_BEGINS_WITH = "$nbw";
        public const string OPERATOR_BEGINS_WITH_ANY = "$bwin";
        public const string OPERATOR_NOT_BEGINS_WITH_ANY = "$nbwin";
        public const string OPERATOR_BLANK = "$blank";
        public const string OPERATOR_GREATER_THAN = "$gt";
        public const string OPERATOR_GREATER_THAN_OR_EQUAL = "$gte";
        public const string OPERATOR_LESS_THAN = "$lt";
        public const string OPERATOR_LESS_THAN_OR_EQUAL = "$lte";
        public const string OPERATOR_SEARCH = "$search";
    }
}