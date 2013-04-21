using NUnit.Framework;

namespace FactualDriver.Tests
{
    class RowQueryTests : QueryBase
    {
        [Test]
        public void RowQuerySerializationTests()
        {
            var query = new RowQuery()
                .Only("name", "address", "region", "postcode");

            //Assert
            AreEqualQueries("select=name,address,region,postcode", query);
        }

        public void AreEqualQueries(string decodedQueryString, RowQuery query)
        {
            Assert.AreEqual(decodedQueryString, DecodeQueryString(query.ToUrlQuery()));
        }    
    }
}
