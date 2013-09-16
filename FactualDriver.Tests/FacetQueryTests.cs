using FactualDriver.Filters;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class FacetQueryTests : QueryBase
    {
        [Test]
        public void FacetQuerySelectCountryAndSearchForStarbucks()
        {
            // Arrange
            var query = new FacetQuery("country").Search("starbucks");

            // Assert
            AreEqualQueries("select=country&q=starbucks", query);
        }

        [Test]
        public void CountStarbucksInTheUSByCityAndState()
        {
            // Arrange
            var query = new FacetQuery("locality", "region")
                .Search("starbucks")
                .Field("country").Equal("US");

            // Assert
            AreEqualQueries("select=locality,region&q=starbucks&filters={\"country\":{\"$eq\":\"US\"}}", query);
        }

        [Test]
        public void CountBusinessesByCategory5KmAroundFactual()
        {
            // Arrange
            var query = new FacetQuery("category")
                .Within(new Circle(34.06018, -118.41835, 5000));

            // Assert
            AreEqualQueries("select=category&geo={\"$circle\":{\"$center\":[34.06018,-118.41835],\"$meters\":5000}}", query);
        }
    }
}