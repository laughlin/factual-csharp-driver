using System.Collections.Generic;
using FactualDriver.Filters;
using NUnit.Framework;
using Newtonsoft.Json;

namespace FactualDriver.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void RowFilterSerializationWithArray()
        {
            // Arrange
            var filter = new RowFilter("locality", "$nin", new[] {"Los Angeles", "Northridge"});

            // Act
            var result = JsonConvert.SerializeObject(filter);

            // Assert
            Assert.AreEqual("{\"locality\":{\"$nin\":[\"Los Angeles\",\"Northridge\"]}}", result);
        }

        [Test]
        public void RowFilterSerializationNumeric()
        {
            // Arrange
            var filter = new RowFilter("rating", "$gte", 7.5);

            // Act
            var result = JsonConvert.SerializeObject(filter);

            // Assert
            Assert.AreEqual("{\"rating\":{\"$gte\":7.5}}", result);
        }

        [Test]
        public void OperatorWithTwoRowFiltersTest()
        {
            // Arrange
            var filter = new ConditionalRowFilter("$and",
                                                  new[]
                                                      {
                                                          new RowFilter("last_name", "$eq", "Smith"),
                                                          new RowFilter("first_name", "$eq", "John")
                                                      });
            // Act
            var result = JsonConvert.SerializeObject(filter);

            // Assert
            Assert.AreEqual("{\"$and\":[{\"last_name\":{\"$eq\":\"Smith\"}},{\"first_name\":{\"$eq\":\"John\"}}]}", result);
        }

        [Test]
        public void CombinedOperatorsTest()
        {
            // Arrange
            var filter = new Dictionary<string, object>
                             {
                                 {
                                     "$and", new object[]
                                                 {
                                                     new RowFilter("first_name", "Suzy"),
                                                     new Dictionary<string, object>()
                                                         {
                                                             {
                                                                 "$or",
                                                                 new object[]
                                                                     {
                                                                         new RowFilter("last_name", "Q"),
                                                                         new RowFilter("last_name", "$blank", true)
                                                                     }
                                                                 }
                                                         }
                                                 }
                                     }
                             };

            // Act
            var result = JsonConvert.SerializeObject(filter);

            // Assert
            Assert.AreEqual("{\"$and\":[{\"first_name\":\"Suzy\"},{\"$or\":[{\"last_name\":\"Q\"},{\"last_name\":{\"$blank\":true}}]}]}", result);
        }

        [Test]
        public void GetFilterSerializationTest()
        {
            // Arrange
            var filter = new GeoFilter(34.06021, -118.41828,5000);

            // Act
            var result = JsonConvert.SerializeObject(filter);

            // Assert
            Assert.AreEqual("{\"$circle\":{\"$center\":[34.06021,-118.41828],\"$meters\":5000}}", result);
        }

        [Test]
        public void SimpleFilterIntegerValueSerializationTest()
        {
            //Arrange 
            var filter = new Filter("limit", 24);

            //Act
            var result = JsonConvert.SerializeObject(filter);

            //Assert
            Assert.AreEqual("24", result);
        }

        [Test]
        public void SimpleFilterStringValueSerializationTest()
        {
            //Arrange 
            var filter = new Filter("search", "Sushi Santa Monica");

            //Act
            var result = JsonConvert.SerializeObject(filter);

            //Assert
            Assert.AreEqual("Sushi Santa Monica", result);
        }

    }
}