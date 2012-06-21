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
        /// <param name="query">IQuery type</param>
        /// <param name="fieldName">Field name on which query is being built.</param>
        public QueryBuilder(T query, string fieldName)
        {
            _query = query;
            _fieldName = fieldName;
        }

        /// <summary>
        /// Adds a new Row filter for specified field in the constructor.
        /// </summary>
        /// <param name="compareOperator">Row filter operator.</param>
        /// <param name="compareValue">Row filter value.</param>
        public void AddFilter(string compareOperator,object compareValue)
        {
            _query.Add(new RowFilter(_fieldName,compareOperator,compareValue));
        }
        
        /// <summary>
        /// Adds a generic search filter.
        /// </summary>
        /// <param name="searchValue">Search value.</param>
        /// <returns>Generic QueryBuilder</returns>
        	public T Search(object searchValue)
        	{
            AddFilter(Constants.OPERATOR_SEARCH, searchValue);
        	    return _query;
        	}

        /// <summary>
        /// Adds a generic equal filter.
        /// </summary>
        /// <param name="value">Value of the filter.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T Equal(object value)
        {
            AddFilter(Constants.OPERATOR_EQUAL, value);
            return _query;
        }

        /// <summary>
        /// Adds a generic not equal filter.
        /// </summary>
        /// <param name="value">Not equal value.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T NotEqual(object value)
        {
            AddFilter(Constants.OPERATOR_NOT_EQUAL, value);
            return _query;
        }

        /// <summary>
        /// Adds a generic "in" filter
        /// </summary>
        /// <param name="values">Parameter array of in values.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T In(params string[] values)
        {
            AddFilter(Constants.OPERATOR_EQUALS_ANY, values);
            return _query;
        }


        /// <summary>
        /// Adds a generic not equals to any filter.
        /// </summary>
        /// <param name="values">Parameter array of values.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T NotIn(params string[] values) {
            AddFilter(Constants.OPERATOR_NOT_EQUALS_ANY, values);
            return _query;
        }

        /// <summary>
        /// Adds a generic begins with filter.
        /// </summary>
        /// <param name="value">Begins with value.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T BeginsWith(string value) {
            AddFilter(Constants.OPERATOR_BEGINS_WITH,value);
            return _query;
        }

        /// <summary>
        /// Adds a generic not begins with filter.
        /// </summary>
        /// <param name="value">Not begins with value.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T NotBeginsWith(string value)
        {
            AddFilter(Constants.OPERATOR_NOT_BEGINS_WITH,value);
            return _query;
        }

        /// <summary>
        /// Adds a generic begins with any filter.
        /// </summary>
        /// <param name="values">Begins with any value.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T BeginsWithAny(params string[] values) {
            AddFilter(Constants.OPERATOR_BEGINS_WITH_ANY,values);
            return _query;
        }

        /// <summary>
        /// Adds a generic filter that does not begins with any specified values.
        /// </summary>
        /// <param name="values">Parameter array of values.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T NotBeginsWithAny(params string[] values)
        {
            AddFilter(Constants.OPERATOR_NOT_BEGINS_WITH_ANY, values);
            return _query;
        }

        /// <summary>
        /// Adds a blank filter.
        /// </summary>
        /// <returns>Generic QueryBuilder</returns>
        public T Blank() {
            AddFilter(Constants.OPERATOR_BLANK, true);
            return _query;
        }

        /// <summary>
        /// Adds a not blank filter.
        /// </summary>
        /// <returns>Generic QueryBuilder</returns>
        public T NotBlank() {
            AddFilter(Constants.OPERATOR_BLANK, false);
            return _query;
        }

        /// <summary>
        /// Adds a greater than filter.
        /// </summary>
        /// <param name="value">Greater than value.</param>
        /// <returns>Generic QueryBuilder</returns>
        public T GreaterThan(object value)
        {
            AddFilter(Constants.OPERATOR_GREATER_THAN, value);
            return _query;
        }

        /// <summary>
        /// Adds a greater than or equal filter.
        /// </summary>
        /// <param name="value">Value of the filter.</param>
        /// <returns>Generic QueryBuilder.</returns>
        public T GreaterThanOrEqual(object value)
        {
            AddFilter(Constants.OPERATOR_GREATER_THAN_OR_EQUAL, value);
            return _query;
        }

        /// <summary>
        /// Adds a less than filter.
        /// </summary>
        /// <param name="value">Value of the filter.</param>
        /// <returns>Generic QueryBuilder.</returns>
        public T LessThan(object value)
        {
            AddFilter(Constants.OPERATOR_LESS_THAN, value);
            return _query;
        }

        /// <summary>
        /// Adds a less than or equal filter.
        /// </summary>
        /// <param name="value">Filter value.</param>
        /// <returns>Generic QueryBuilder.</returns>
        public T LessThanOrEqual(object value)
        {
            AddFilter(Constants.OPERATOR_LESS_THAN_OR_EQUAL, value);
            return _query;
        }

    }
}