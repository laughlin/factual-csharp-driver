using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class ResoleQueryTests: QueryBase
    {
        [Test]
        public void ResolveQuerySerializationTests()
        {
            var query = new ResolveQuery()
                .Add("name", "McDonalds")
                .Add("address", "10451 Santa Monica Blvd")
                .Add("region", "CA")
                .Add("postcode","90025");

            // Assert
            AreEqualQueries("values={\"name\":\"McDonalds\",\"address\":\"10451 Santa Monica Blvd\",\"region\":\"CA\",\"postcode\":\"90025\"}&debug=false",
                query);
        }

        [Test]
        public void ResolveQuerySerializationTestsPosition()
        {
            var query = new ResolveQuery()
                .Add("name", "Buena Vista")
                .Add("latitude", 34.06)
                .Add("longitude", -118.40);

            // Assert
            AreEqualQueries("values={\"name\":\"Buena Vista\",\"latitude\":\"34.06\",\"longitude\":\"-118.40\"}&debug=false",query);
        }

        [Test]
        public void ResolveQuerySerializationTestDebugModeEnabled()
        {
            var query = new ResolveQuery()
               .Add("name", "Buena Vista")
               .Add("latitude", 34.06)
               .Add("longitude", -118.40)
               .EnableDebugMode(true);

            // Assert
            AreEqualQueries("values={\"name\":\"Buena Vista\",\"latitude\":\"34.06\",\"longitude\":\"-118.40\"}&debug=true", query);

        }

        [Test]
        public void ResolveQuerySerializationTestDebugModeDisabled()
        {
            var query = new ResolveQuery()
               .Add("name", "Buena Vista")
               .Add("latitude", 34.06)
               .Add("longitude", -118.40)
               .EnableDebugMode(false);

            // Assert
            AreEqualQueries("values={\"name\":\"Buena Vista\",\"latitude\":\"34.06\",\"longitude\":\"-118.40\"}&debug=false", query);

        }

        public void AreEqualQueries(string decodedQueryString, ResolveQuery query)
        {
            Assert.AreEqual(decodedQueryString, DecodeQueryString(query.ToUrlQuery()));
        }
    }
}