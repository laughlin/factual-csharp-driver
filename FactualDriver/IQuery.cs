using FactualDriver.Filters;

namespace FactualDriver
{
    /// <summary>
    /// Base interface for query
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        ///Add filter to the query
        /// </summary>
        /// <param name="filter">Filter to add</param>
        void Add(IFilter filter);

        /// <summary>
        /// Convert query to url encoded path and query that works with Factual
        /// </summary>
        /// <returns></returns>
        string ToUrlQuery();
    }
}