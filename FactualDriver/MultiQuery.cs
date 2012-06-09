using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace FactualDriver
{
    public class MultiQuery
    {
        /// <summary>
        /// Collection of query keys and full url paths
        /// </summary>
        Dictionary<string, string> _queries = new Dictionary<string, string>();

        /// <summary>
        /// We start query count at one per driver documentation 
        /// </summary>
        private int queryCount = 1;

        public void AddQuery(string path, string query)
        {
            _queries.Add("query" + queryCount, string.Format("{0}?{1}", path, query));
            queryCount++;
        }

        public string ToUrlQuery()
        {
            var queries = string.Join(",", _queries.Select(p => string.Format("\"{0}\":\"/{1}\"", p.Key, p.Value)));
            return "queries=" + HttpUtility.UrlEncode("{" + queries + "}");
        }
    }
}