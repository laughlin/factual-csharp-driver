using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class CrosswalkQueryTests : QueryBase
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

    }
}