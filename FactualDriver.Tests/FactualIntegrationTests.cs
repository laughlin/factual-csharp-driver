using System.Collections.Generic;
using System.Configuration;
using FactualDriver.Filters;
using FactualDriver.Utils;
using NUnit.Framework;
using Newtonsoft.Json;

namespace FactualDriver.Tests
{
    [TestFixture]
    public class FactualIntegrationTests
    {
        private static readonly string OAuthKey = ConfigurationManager.AppSettings["oAuthKey"];
        private static readonly string OAuthSecret = ConfigurationManager.AppSettings["oAuthSecret"];
        public Factual Factual { get; set; }

        [SetUp]
        public void Init()
        {
            if (string.IsNullOrWhiteSpace(OAuthKey) || string.IsNullOrEmpty(OAuthSecret))
                throw new ConfigurationException("please specify oauth keys");

            Factual = new Factual(OAuthKey, OAuthSecret);
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
            var filter = new ConditionalRowFilter("$and", new[]
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

 

    }
}