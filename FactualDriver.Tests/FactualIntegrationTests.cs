using System.Collections.Generic;
using System.Configuration;
using FactualDriver.Filters;
using FactualDriver.Utils;
using NUnit.Framework;
using Newtonsoft.Json;

namespace FactualDriver.Tests
{
    [TestFixture]
    public class FactualIntegrationTests : FactualIntegrationBase
    {

        [Test]
        public void TestSchema()
        {
            //Arrange
            var result = Factual.Schema("restaurants-us");
            dynamic json = JsonConvert.DeserializeObject(result);
            //Assert
            Assert.AreEqual("ok", (string)json.status);
        }

        [Test]
        public void GeopulseTest()
        {
            //Arrange
            var result =
                Factual.Geopulse(new Geopulse(new Point(34.06021, -118.41828)).Only("commercial_density",
                                                                                    "commercial_profile"));
            dynamic json = JsonConvert.DeserializeObject(result);
            //Assert
            Assert.AreEqual("ok", (string)json.status);
        }

        [Test]
        public void TestReverseGeocode()
        {
            //Arrange
            var result = Factual.ReverseGeocode(new Point(34.06021, -118.41828));

            //Assert
            AssertReceivedOkResponse(result);
        }

        [Test]
        public void TestMultiQuery()
        {
            //Arrange
            Factual.QueueFetch("places", new Query().Field("region").Equal("CA"));
            Factual.QueueFetch("places", new Query().Limit(1));
            //Act
            var result = Factual.SendQueueRequests();
            //Assert
            dynamic json = JsonConvert.DeserializeObject(result);
            //Assert
            Assert.AreEqual("ok", (string)json.query1.status);
        }

        [Test]
        public void RawQueryTest()
        {
            // Arrange
            var rawParameters = "filters={\"name\":{\"$bw\":\"Star\"}}&include_count=true";

            // Act
            string result = Factual.RawQuery("t/global", rawParameters);
            dynamic json = JsonConvert.DeserializeObject(result);

            // Assert
            Assert.AreEqual("ok", (string)json.status);
        }


        [Test]
        public void QueryWithSimpleRowFilter()
        {
            // Arrange
            var filter = new RowFilter("country", "US");

            // Act
            string result = Factual.Query("t/global", filter);
            dynamic json = JsonConvert.DeserializeObject(result);

            // Assert
            Assert.AreEqual("ok", (string)json.status);
        }

        //"filters={\"name\":{\"$bw\":\"Star\"}}"
        [Test]
        public void QueryWithBeginFilter()
        {
            // Arrange
            var filter = new RowFilter("name", "$bw", "$Star");
            
            // Act
            string result = Factual.Query("t/global", filter);
            dynamic json = JsonConvert.DeserializeObject(result);
            // Assert
            Assert.AreEqual("ok",(string)json.status);
        }

        [Test]
        public void QueryWithGeoFilter()
        {
            // Arrange
            var filter = new GeoFilter(34.06018, -118.41835, 500);

            // Act
            string result = Factual.Query("t/global", filter);
            dynamic json = JsonConvert.DeserializeObject(result);

            // Assert
            Assert.AreEqual("ok", (string)json.status);
        }

        [Test]
        public void FullTextSearch()    
        {
            // Arrange
            var filter = new SearchFilter("vegan,Los Angeles");

            // Act
            string result = Factual.Query("t/restaurants-us", filter);
            dynamic json = JsonConvert.DeserializeObject(result);

            // Assert
            Assert.AreEqual("ok", (string)json.status);
        }


        [Test]
        public void MultipleFiltersTest()
        {
            // Arrange
            var filter = new RowFilter("name", "$bw", "Star");
            var filter2 = new Filter("include_count", "true");

            // Act
            string result = Factual.Query("t/restaurants-us", filter, filter2);
            dynamic json = JsonConvert.DeserializeObject(result);

            // Assert
            Assert.AreEqual("ok", (string)json.status);
        }

        [Test]
        public void GeoAndRowFilterTest()
        {
            // Arrange
            var filter = new RowFilter("name", "Stand");
            var filter2 = new GeoFilter(34.06018, -118.41835, 5000);

            // Act
            string result = Factual.Query("t/restaurants-us", filter, filter2);
            dynamic json = JsonConvert.DeserializeObject(result);

            // Assert
            Assert.AreEqual("ok", (string)json.status);
        }


        [Test]
        public void ParametersWithConditionalOperators()
        {
            // Arrange
            var filter = new FilterGroup(new List<IFilter>
                                                              {
                                                                  new RowFilter("name", "$search", "McDonald's"),
                                                                  new RowFilter("category", "$bw", "Food & Beverage")
                                                              });



            // Act
            string result = Factual.Query("t/global", filter);
            dynamic json = JsonConvert.DeserializeObject(result);

            // Assert
            Assert.AreEqual("ok", (string)json.status);
        }

 
        public void  AssertReceivedOkResponse(string result)
        {
            dynamic json = JsonConvert.DeserializeObject(result);
            //Assert
            Assert.AreEqual("ok", (string)json.status);
        }
    }
}