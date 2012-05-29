using FactualDriver.Filters;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    [TestFixture]
    public class QueryTests
    {
        [Test]
        public void LimitTest()
        {
            //Assert
            Assert.AreEqual("limit=24", new Query().Limit(24).ToUrlQuery());
        }

        [Test]
        public void SearchTest()
        {
            Assert.AreEqual("q=Sushi%27s+man+Santa+Monica", new Query().Search("Sushi's man Santa Monica").ToUrlQuery());
        }

        [Test]
        public void GeoCirlceTest()
        {
            Assert.AreEqual(
                "geo=%7b%22%24circle%22%3a%7b%22%24center%22%3a%5b34.06021%2c-118.41828%5d%2c%22%24meters%22%3a5000%7d%7d",
                new Query().WithIn(new Circle(34.06021, -118.41828, 5000)).ToUrlQuery());
        }

        [Test]
        public void TestCommaSeparatedQueries()
        {
            var query = new Query().SortAsc("name");

            //sort=name:asc
            Assert.AreEqual("sort=name%3aasc", query.ToUrlQuery());
            
            //sort=name:asc,country:desc
            Assert.AreEqual("sort=name%3aasc%2ccountry%3adesc", query.SortDesc("country").ToUrlQuery()); 
        }
    }
}