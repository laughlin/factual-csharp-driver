using FactualDriver.Filters;

namespace FactualDriver
{
    /// <summary>
    /// Provides fluent interface to specifying row filter predicate logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryBuilder<T> where T : IQuery 
    {
        private readonly T _query;
        private readonly string _fieldName;

        /// <summary>
        /// Constructor. Specifies the name of the field for which to build filter
        /// logic. Instance methods are used to specify the desired logic.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="fieldName"></param>
        public QueryBuilder(T query, string fieldName)
        {
            _query = query;
            _fieldName = fieldName;
        }

        public void AddFilter(string compareOperator,object compareValue)
        {
            _query.Add(new RowFilter(_fieldName,compareOperator,compareValue));
        }
        
        	public T Search(object searchValue)
        	{
            AddFilter(Constants.OPERATOR_SEARCH, searchValue);
        	    return _query;
        	}

        public T Equal(object value)
        {
            AddFilter(Constants.OPERATOR_EQUAL, value);
            return _query;
        }

        public T NotEqual(object value)
        {
            AddFilter(Constants.OPERATOR_NOT_EQUAL, value);
            return _query;
        }

        public T In(params string[] values)
        {
            AddFilter(Constants.OPERATOR_EQUALS_ANY, values);
            return _query;
        }


        public T NotIn(params string[] values) {
            AddFilter(Constants.OPERATOR_NOT_EQUALS_ANY, values);
            return _query;
        }

 
        public T BeginsWith(string value) {
            AddFilter(Constants.OPERATOR_BEGINS_WITH,value);
            return _query;
        }

        public T NotBeginsWith(string value)
        {
            AddFilter(Constants.OPERATOR_NOT_BEGINS_WITH,value);
            return _query;
        }

        public T BeginsWithAny(params string[] values) {
            AddFilter(Constants.OPERATOR_BEGINS_WITH_ANY,values);
            return _query;
        }

        public T NotBeginsWithAny(params string[] values)
        {
            AddFilter(Constants.OPERATOR_NOT_BEGINS_WITH_ANY, values);
            return _query;
        }

        public T Blank() {
            AddFilter(Constants.OPERATOR_BLANK, true);
            return _query;
        }

        public T NotBlank() {
            AddFilter(Constants.OPERATOR_BLANK, false);
            return _query;
        }

        public T GreaterThan(object value)
        {
            AddFilter(Constants.OPERATOR_GREATER_THAN, value);
            return _query;
        }

        public T GreaterThanOrEqual(object value)
        {
            AddFilter(Constants.OPERATOR_GREATER_THAN_OR_EQUAL, value);
            return _query;
        }

        public T LessThan(object value)
        {
            AddFilter(Constants.OPERATOR_LESS_THAN, value);
            return _query;
        }

        public T LessThanOrEqual(object value)
        {
            AddFilter(Constants.OPERATOR_LESS_THAN_OR_EQUAL, value);
            return _query;
        }

    }
}