using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class CrosswalkQueryTests
    {
        [Test]
        public void CrosswalkQueryTest()
        {
            //Arrange
            var query = new CrosswalkQuery()
                .FactualId("97598010-433f-4946-8fd5-4a6dd1639d77")
                .Limit(100)
                .Namespace("foursquare")
                .NamespaceId("443338")
                .Only("foursquare", "yelp");

            //Assert
            AreEqualQueries("factual_id=97598010-433f-4946-8fd5-4a6dd1639d77&limit=100&namespace=foursquare&namespace_id=443338&only=foursquare,yelp",query);
        }

        public void AreEqualQueries(string decodedQueryString, CrosswalkQuery query)
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