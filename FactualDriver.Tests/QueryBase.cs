using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class QueryBase
    {
        public void AreEqualQueries(string decodedQueryString, IQuery query)
        {
            Assert.AreEqual(decodedQueryString, DecodeQueryString(query.ToUrlQuery()));
        }

        /// <summary>
        /// Encodes everything except = and &
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string DecodeQueryString(string path)
        {
            var decodedQueries = new List<string>();
            foreach (var query in path.Split('&'))
            {
                decodedQueries.Add(string.Join("=", query.Split('=').Select(HttpUtility.UrlDecode)));
            }

            return string.Join("&", decodedQueries);
        }
    }
}