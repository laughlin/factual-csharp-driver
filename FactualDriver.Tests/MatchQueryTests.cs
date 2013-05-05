using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class MatchQueryTests : QueryBase
    {
        [Test]
        public void MatchQuerySerializationTests()
        {
            var query = new MatchQuery()
                .Add("name", "McDonalds")
                .Add("address", "10451 Santa Monica Blvd")
                .Add("region", "CA")
                .Add("postcode", "90025");

            //Assert
            AreEqualQueries("values={\"name\":\"McDonalds\",\"address\":\"10451 Santa Monica Blvd\",\"region\":\"CA\",\"postcode\":\"90025\"}",
                query);
        }

        [Test]
        public void MatchQuerySerializationTestsPosition()
        {
            var query = new MatchQuery()
                .Add("name", "Buena Vista")
                .Add("latitude", 34.06)
                .Add("longitude", -118.40);

            //Assert
            AreEqualQueries("values={\"name\":\"Buena Vista\",\"latitude\":\"34.06\",\"longitude\":\"-118.40\"}", query);
        }

        public void AreEqualQueries(string decodedQueryString, MatchQuery query)
        {
            Assert.AreEqual(decodedQueryString, DecodeQueryString(query.ToUrlQuery()));
        }
    }
}