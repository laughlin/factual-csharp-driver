using System;
using System.Linq;
using System.Collections.Generic;
using FactualDriver.Exceptions;
using FactualDriver.Filters;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FactualDriver.Tests
{
    [TestFixture]
    public class FactualIntegrationTests : FactualIntegrationBase
    {
        public const double Latitude = 34.06018;
        public const double Longitude = -118.41835;
        public const int Meters = 5000;

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
                Factual.Geopulse(new Geopulse(new Point(34.06021, -118.41828)).Only("income",
                                                                                    "housing"));
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
            Assert.AreEqual("ok", (string)json.q0.status);
            Assert.AreEqual("ok", (string)json.q1.status);
        }

        [Test]
        public void TestMultiQueryWithCustomKey()
        {
            //Arrange
            Factual.MultiQuery.Key = "test";
            Factual.QueueFetch("places", new Query().Field("region").Equal("CA"));
            Factual.QueueFetch("places", new Query().Limit(1));
            
            //Act
            var result = Factual.SendQueueRequests();

            //Assert
            dynamic json = JsonConvert.DeserializeObject(result);
            Assert.AreEqual("ok", (string)json.test0.status);
            Assert.AreEqual("ok", (string)json.test1.status);
        }

        [Test]
        public void TestMultiQueryWithMonetize()
        {
            //Arrange
            Factual.QueueFetch("places", new Query().Field("region").Equal("CA"));
            Factual.QueueFetch("places", new Query().Limit(1));
            Factual.QueueFetchMonetize(new Query().Field("place_locality").Equal("Los Angeles"));

            //Act
            var result = Factual.SendQueueRequests();

            //Assert
            dynamic json = JsonConvert.DeserializeObject(result);
            Assert.AreEqual("ok", (string)json.q0.status);
            Assert.AreEqual("ok", (string)json.q1.status);
            Assert.AreEqual("ok", (string)json.q2.status);
        }

        [Test]
        public void TestMultiComplex()
        {
            //Arrange
            Factual.QueueFetch("global", new FacetQuery("region", "locality"));
            Factual.QueueFetch("places", new Query().Limit(1));
            Factual.QueueFetch("places", new ResolveQuery()
                                             .Add("name", "McDonalds")
                                             .Add("address", "10451 Santa Monica Blvd")
                                             .Add("region", "CA")
                                             .Add("postcode", "90025"));
            //Act
            var response = Factual.SendQueueRequests();

            //Assert
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.AreEqual("ok", (string)json.q0.status);
            Assert.AreEqual("ok", (string)json.q1.status);
            Assert.AreEqual("ok", (string)json.q2.status);
        }

        [Test]
        public void TestMultiCrosswalk()
        {
            Factual.QueueFetch("crosswalk", new Query()
                                             .Field("factual_id").Equal("97598010-433f-4946-8fd5-4a6dd1639d77")
                                             .Limit(1));
            //Act
            var response = Factual.SendQueueRequests();
            //Assert
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.AreEqual("ok", (string)json.q0.status);
        }

        [Test]
        public void TestMiltiGeopulseWithNearestAddress()
        {
            //Arrange
            Factual.QueueFetch(new Point(Latitude,Longitude));
            Factual.QueueFetch(new Geopulse(new Point(Latitude,Longitude)));
            //Act
            var response = Factual.SendQueueRequests();
            //Assert
            dynamic json = JsonConvert.DeserializeObject(response);
            var results = (ICollection<JToken>) json;
            Assert.AreEqual(2,results.Count);
            Assert.AreEqual("ok", (string)json.q0.status);
            Assert.AreEqual("ok", (string)json.q1.status);
        }

        [Test]
        public void TestMultiGeopulseWithNearestPlace()
        {
            //Arrange
            Factual.QueueFetch("global", new Query().WithIn(new Circle(Latitude, Longitude, Meters)));
            Factual.QueueFetch(new Geopulse(new Point(Latitude, Longitude)));
            //Act
            var response = Factual.SendQueueRequests();
            //Assert
            dynamic json = JsonConvert.DeserializeObject(response);
            var results = (ICollection<JToken>)json;
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("ok", (string)json.q0.status);
            Assert.AreEqual("ok", (string)json.q1.status);
        }

        [Test]
        public void TestMonetize()
        {
            //Arrange
            var response = Factual.Monetize(new Query().Field("place_locality").Equal("Los Angeles"));

            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestMonetizeByBusiness()
        {
            //Arrange
            var response = Factual.Monetize(new Query().Field("place_factual_id").Equal("3226fac0-2f85-49d7-bc67-288fb2fc52ee"));

            //Assert
            AssertReceivedOkResponse(response);
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

        //Port of factual java driver tests

        [Test]
        public void TestCoreExample1()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query().Field("country").Equal("US"));

            //Assert
            AssertReceivedOkResponse(response);
        }

        /// <summary>
        /// Find rows in the restaurant database whose name begins with "Star" and
        /// return both the data and a total count of the matched rows.
        /// </summary>
        [Test]
        public void TestCoreExample2()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query()
                                                       .Field("name")
                                                       .BeginsWith("Star")
                                                       .IncludeRowCount());
            var raw = Factual.RawQuery("t/places", "filters={\"name\":{\"$bw\":\"Star\"}}&include_count=true");

            //Assert
            AssertReceivedOkResponse(response);
            Assert.AreEqual(response, raw);

        }

        /// <summary>
        /// Do a full-text search of the restaurant database for rows that match the
        /// terms "Fried Chicken, Los Angeles"
        /// </summary>
        [Test]
        public void TestCoreExample3()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query().Search("Fried Chicken, Los Angeles"));
            Factual.Debug = true;
            var raw = Factual.RawQuery("t/places", "q=Fried Chicken, Los Angeles");
            var test2 = Factual.RawQuery("t/places", "q=Fried Chicken%2C Los Angeles");
            //Assert
            AssertReceivedOkResponse(response);
            Assert.AreEqual(test2, raw);
            Assert.AreEqual(response, raw);
            
        }

        /// <summary>
        /// Do a full-text search of the the global database with non-english search charachters
        /// terms "משה"
        /// </summary>
        [Test]
        public void TestCoreExample3NonEnglish()
        {
            //Arrange & Act
            var response = Factual.Fetch("global", new Query().Search("משה"));
            Factual.Debug = true;

            //Assert
            AssertReceivedOkResponse(response);
        }

        /// <summary>
        /// To support paging in your app, return rows 20-25 of the full-text search result
        /// from Example 3
        /// </summary>
        [Test]
        public void TestCoreExample4()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query()
                .Search("Fried Chicken, Los Angeles")
                .Offset(20)
                .Limit(5));
            dynamic jsonResponse = JsonConvert.DeserializeObject(response);
            var raw = Factual.RawQuery("t/places", "q=Fried Chicken, Los Angeles&offset=20&limit=5");

            //Assert
            AssertReceivedOkResponse(response);
            Assert.AreEqual(5, ((ICollection<JToken>)jsonResponse.response.data).Count);
            Assert.AreEqual(response, raw);
        }

        /// <summary>
        /// Return rows from the global places database with a name equal to "Stand"
        /// within 5000 meters of the specified lat/lng
        /// </summary>
        [Test]
        public void TestCoreExample5()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query()
                                                       .Field("name").Equal("Stand")
                                                       .WithIn(new Circle(Latitude, Longitude, Meters)));

            var raw = Factual.RawQuery("t/places", "filters={\"name\":{\"$eq\":\"Stand\"}}&geo={\"$circle\":{\"$center\":[34.06018,-118.41835],\"$meters\":5000}}");

            //Assert
            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            Assert.AreEqual(response, raw);
        }

        [Test]
        public void TestSortByDistance()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query()
                                                       .WithIn(new Circle(Latitude, Longitude, Meters))
                                                       .SortAsc("$distance"));

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            AssertAscendingDoubles(response, "$distance");

            var raw = Factual.RawQuery("t/places", "geo={\"$circle\":{\"$center\":[34.06018,-118.41835],\"$meters\":5000}}&sort=$distance:asc");

            //Assert

            Assert.AreEqual(response, raw);
        }


        [Test]
        public void TestRowFiltersTwoBeginsWith()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query()
                                                       .Field("name").BeginsWith("McDonald's")
                                                       .Field("category").BeginsWith("Food & Beverage"));

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            AssertStartsWith(response, "name", "McDonald");
            AssertStartsWith(response, "category", "Food & Beverage");

            var raw = Factual.RawQuery("t/places", "filters={\"$and\":[{\"name\":{\"$bw\":\"McDonald's\"}},{\"category\":{\"$bw\":\"Food %26 Beverage\"}}]}");

            //Assert

            Assert.AreEqual(response, raw);
        }

        [Test]
        public void TestIn()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query()
                                                       .Field("region").In("CA","NM","FL"));

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            AssertIn(response, "region", "CA", "NM", "FL");

            var raw = Factual.RawQuery("t/places", "filters={\"region\":{\"$in\":[\"CA\",\"NM\",\"FL\"]}}");

            //Assert

            Assert.AreEqual(response, raw);
        }

        [Test]
        public void TestNestedOrWithinTheTopLevelAnd()
        {
            //Arrange & Act
            var query = new Query().Field("region").In("MA", "VT", "NH");
            query.Or
                (
                    query.Field("name").BeginsWith("Coffee"),
                    query.Field("name").BeginsWith("Star")
                );

            var response = Factual.Fetch("places", query);

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            AssertStartsWithEither(response, "name", "Coffee", "Star");

            var raw = Factual.RawQuery("t/places", "filters={\"$and\":[{\"region\":{\"$in\":[\"MA\",\"VT\",\"NH\"]}},{\"$or\":[{\"name\":{\"$bw\":\"Coffee\"}},{\"name\":{\"$bw\":\"Star\"}}]}]}");

            //Assert

            Assert.AreEqual(response, raw);
        }

        [Test]
        public void TestSimpleTel()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query().Field("tel").BeginsWith("(212)"));

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            AssertStartsWith(response,"tel","(212)");
        }

        [Test]
        public void TestFullTextSaerchOnAField()
        {
            //Arrange & Act
            var response = Factual.Fetch("places", new Query().Field("name").BeginsWith("Fried Chicken"));

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);

            dynamic json = JsonConvert.DeserializeObject(response);
            foreach (var value in (ICollection<JToken>)json.response.data)
            {
                var responseValue = (string)value["name"];
                Assert.IsTrue(responseValue.ToLower().Contains("frie") || responseValue.ToLower().Contains("fry")
                    || responseValue.ToLower().Contains("chicken"));
            }
        }

        [Test]
        public void TestCrosswalkEx1()
        {
            //Arrange & Act
            var response = Factual.Fetch("crosswalk", new Query().Field("factual_id").Equal("97598010-433f-4946-8fd5-4a6dd1639d77"));

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            AssertCrosswalkResult(response, "factual_id", "97598010-433f-4946-8fd5-4a6dd1639d77");
            
        }

        [Test]
        public void TestCrosswalkEx2()
        {
            //Arrange & Act
            var response = Factual.Fetch("crosswalk",
                                         new Query().Field("factual_id").Equal("97598010-433f-4946-8fd5-4a6dd1639d77"));

            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            AssertCrosswalkResult(response, "factual_id", "97598010-433f-4946-8fd5-4a6dd1639d77");
        }

        [Test]
        public void TestCrosswalkEx3()
        {
            //Arrange
            var response = Factual.Fetch("crosswalk", new Query().Field("namespace").Equal("foursquare")
                                                       .Field("namespace_id").Equal("4ae4df6df964a520019f21e3"));
            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
        }


        [Test]
        public void TestCrosswalkLimit()
        {
            //Arrange
            var response = Factual.Fetch("crosswalk", new Query().Field("factual_id").Equal("97598010-433f-4946-8fd5-4a6dd1639d77").Limit(1));
            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsTrue(((int)json.response.included_rows) == 1);
        }

        [Test]
        public void TestResolveEx1()
        {
            //Arrange & Act
            var response =
                  Factual.Fetch("places", new ResolveQuery()
                  .Add("name", "McDonalds")
                  .Add("address", "10451 Santa Monica Blvd")
                  .Add("region", "CA")
                  .Add("postcode", "90025"));
            //Assert
            AssertReceivedOkResponse(response);
            AssertNotEmpty(response);
        }

        [Test]
        public void TestSelectFieldsOnly()
        {
            //Arrange
            var response = Factual.Fetch("places", new Query().Field("country").Equal("US").Only("address","country"));
            AssertReceivedOkResponse(response);
            AssertAll(response, "country", "us");

            var raw = Factual.RawQuery("t/places", "filters={\"country\":{\"$eq\":\"US\"}}&select=address,country");

            //Assert

            Assert.AreEqual(response, raw);
        }

        /// <summary>
        /// And should not be used for geo queries.
        /// However, neither should it throw an exception.
        /// </summary>
        [Test]
        public void TestInvalidAnd()
        {
            //Arrange
            var query = new Query();
            query.And
                (
                    query.Field("category").BeginsWith("Food"),
                    query.WithIn(new Circle(Latitude, Longitude, Meters))
                );
            //Act
            var response = Factual.Fetch("places", query);
            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestFacet()
        {
            //Arrange
            var facet = new FacetQuery("region", "locality")
                .Search("Starbucks")
                .MaxValuesPerFacet(20)
                .MinCountPerFacetValue(100)
                .IncludeRowCount();

            //Act
            var response = Factual.Fetch("global", facet);

            //Assert
            AssertReceivedOkResponse(response);
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsTrue(((int)json.response.total_row_count) > 0);
        }

        [Test]
        public void TestFacetFilter()
        {
            //Arrange
            var facet = new FacetQuery("locality")
                .Field("region").In("MA", "VT", "NH");
            facet.And
                (
                    facet.Or
                        (
                            facet.Field("name").BeginsWith("Coffee"),
                            facet.Field("name").BeginsWith("Star")
                        ),
                    facet.Field("locality").BeginsWith("w")
                );
            //Act
            var response = Factual.Fetch("global", facet);

            //Assert
            AssertReceivedOkResponse(response);
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsTrue(((ICollection<JToken>)json.response.data.locality).Count > 0);
        }

        [Test]
        public void TestFacetGeo()
        {
            //Arrange
            var facet = new FacetQuery("category").Within(new Circle(Latitude, Longitude, Meters));
            //Act
            var response = Factual.Fetch("global", facet);
            //Assert
            AssertReceivedOkResponse(response);
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsTrue(((ICollection<JToken>)json.response.data.category).Count > 0);
        }

        [Test]
        public void TestGeopulse()
        {
            //Arrange
            var response = Factual.Geopulse(new Geopulse(new Point(Latitude, Longitude))
                .Only("income", "area_statistics"));
            dynamic json = JsonConvert.DeserializeObject(response);
            var pulse = json.response.data.demographics;
            //Assert
            AssertReceivedOkResponse(response);
            Assert.IsTrue(pulse["income"] != null);
            Assert.IsTrue(pulse["area_statistics"] != null);
        }

        [Test]
        public void TestGeocode()
        {
            //Arrange
            var response = Factual.ReverseGeocode(new Point(Latitude, Longitude));
            dynamic json = JsonConvert.DeserializeObject(response);
            //Assert
            AssertReceivedOkResponse(response);

            Assert.IsTrue(((ICollection<JToken>) json.response.data).Count > 0);
        }

        [Test]
        public void TestWorldGeographies()
        {
            //Arrange
            var query = new Query();
                query.And
                (
                    query.Field("name").Equal("philadelphia"),
                    query.Field("country").Equal("us"),
                    query.Field("placetype").Equal("locality")
                );
            //Act
            var response = Factual.Fetch("world-geographies", query);

            //Assert
            AssertReceivedOkResponse(response);
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsTrue(((int)json.response.included_rows) == 14);
        }

        [Test]
        public void TestFlagDuplicate()
        {
            //Arrange
            var response = Factual.FlagDuplicate("us-sandbox", "2ca7228a-a77c-448e-a96f-3e1731573fff", new Metadata().User("test_driver_user"));
           
            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestFlagInaccurate()
        {
            //Arrange
            var response = Factual.FlagInaccurate("us-sandbox", "2ca7228a-a77c-448e-a96f-3e1731573fff", new Metadata().User("test_driver_user"));
            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestFlagInappropriate()
        {
            //Arrange
            var response = Factual.FlagInappropriate("us-sandbox", "2ca7228a-a77c-448e-a96f-3e1731573fff", new Metadata().User("test_driver_user"));
            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestFlagNonExistent()
        {
            //Arrange
            var response = Factual.FlagNonExistent("us-sandbox", "2ca7228a-a77c-448e-a96f-3e1731573fff", new Metadata().User("test_driver_user"));
            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestFlagSpam()
        {
            //Arrange
            var response = Factual.FlagSpam("us-sandbox", "2ca7228a-a77c-448e-a96f-3e1731573fff", new Metadata().User("test_driver_user"));
            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestFlagOther()
        {
            //Arrange
            var response = Factual.FlagOther("us-sandbox", "2ca7228a-a77c-448e-a96f-3e1731573fff", new Metadata().User("test_driver_user"));
            //Assert
            AssertReceivedOkResponse(response);
        }

        //[Test]
        public void TestDiffs()
        {
            //Arrange
            DiffsQuery diff = new DiffsQuery(1339123455775);

            //Act
            string response = Factual.Fetch("2EH4Pz", diff);
            //Assert
            AssertReceivedOkResponse(response);
        }

        [Test]
        public void TestSubmitAdd()
        {
            CreateNewEntity();
        }

        /// <summary>
        /// http://support.factual.com/factual/topics/submit_api_using_c_driver
        /// </summary>
        [Test]
        public void SubmitAddCase()
        {
            //Arrange
            var submit = new Submit();
            submit.AddValue("name", "Subway");
            submit.AddValue("address", "22000 Burbank Blvd");
            submit.AddValue("locality", "Northridge");
            submit.AddValue("region", "Los Angeles");
            submit.AddValue("postcode", 91367);
            
            //Act
            var response = Factual.Submit("us-sandbox", submit, new Metadata().User("test_driver_user"));

            //Assert
            AssertReceivedOkResponse(response);
        }

        public string CreateNewEntity()
        {
            //Arrange
            Submit submit = new Submit()
                .AddValue("longitude", 100);
            var response = Factual.Submit("us-sandbox", submit, new Metadata().User("test_driver_user"));

            //Asert
            AssertReceivedOkResponse(response);
            dynamic newEntityjson = JsonConvert.DeserializeObject(response);
            var newEntityId = (string)newEntityjson.response.factual_id;
            return newEntityId;
        }

        [Test]
        public void TestSubmitEdit()
        {
            var newEntityId = CreateNewEntity();

            //Arrange
            Submit submit = new Submit()
                .AddValue("longitude", 101);
            var response = Factual.Submit("us-sandbox", newEntityId, submit, new Metadata().User("test_driver_user"));

            //Asert
            AssertReceivedOkResponse(response);
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsFalse((bool)json.response.new_entity);

        }

        [Test]
        public void TestSubmitDelete()
        {
            var newEntityId = CreateNewEntity();
            //Arrange
            Submit submit = new Submit()
                .RemoveValue("longitude");
            var response = Factual.Submit("us-sandbox", newEntityId, submit, new Metadata().User("test_driver_user"));

            //Asert
            AssertReceivedOkResponse(response);
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsFalse((bool)json.response.new_entity);
        }

        //[Test] per Aaron: Factual doesn't check if id already exists, so it does not result in an error.
        public void TestSubmitError()
        {
            //Arrange
            Submit submit = new Submit()
                .RemoveValue("longitude");

            var exception = Assert.Throws<FactualApiException>(
                () => Factual.Submit("us-sandbox", "randomwrongid", submit, new Metadata().User("test_driver_user")));
            //Asert
            Assert.IsNotNull(exception);
        }

        private void AssertAll(string response, string key, string valueToCheck)
        {
            dynamic json = JsonConvert.DeserializeObject(response);
            Assert.IsTrue(((ICollection<JToken>) json.response.data).All(p => ((string) p[key]).ToLower() == valueToCheck.ToLower()));

        }

        private void AssertCrosswalkResult(string response, string key, string id)
        {
            dynamic json = JsonConvert.DeserializeObject(response);
            foreach (var value in (ICollection<JToken>)json.response.data)
            {
                var responseValue = (string)value[key];
                Assert.IsTrue(responseValue == id);
            }
        }

        private void AssertStartsWithEither(string response, string key, string value1, string value2)
        {
            dynamic json = JsonConvert.DeserializeObject(response);
            foreach (var value in (ICollection<JToken>)json.response.data)
            {
                var responseValue = (string) value[key];
                Assert.IsTrue(responseValue.StartsWith(value1) || responseValue.StartsWith(value2));
            }
        }

        private void AssertIn(string response, string key, params string[] values)
        {
            dynamic json = JsonConvert.DeserializeObject(response);
            foreach (var value in (ICollection<JToken>)json.response.data)
            {
                Assert.IsTrue(values.Contains((string)value[key]));
            }
        }

        private void AssertStartsWith(string response, string key, string valueToCheck)
        {
            dynamic json = JsonConvert.DeserializeObject(response);
            foreach (var value in (ICollection<JToken>)json.response.data)
            {
                Assert.IsTrue(((string)value[key]).StartsWith(valueToCheck, StringComparison.OrdinalIgnoreCase));
            }
        }

        private void AssertAscendingDoubles(string response, string valueToCheck)
        {
            double previous = 0;
            dynamic json = JsonConvert.DeserializeObject(response);

            foreach (var value in (ICollection<JToken>)json.response.data)
            {
                var currentValue = (double) value[valueToCheck];
                Assert.IsTrue(currentValue >= previous);
                previous = currentValue;
            }
        }

        public void  AssertReceivedOkResponse(string result)
        {
            dynamic json = JsonConvert.DeserializeObject(result);
            //Assert
            Assert.AreEqual("ok", (string)json.status);
        }

        public void AssertNotEmpty(string result)
        {
            dynamic json = JsonConvert.DeserializeObject(result);
            Assert.IsTrue(((int)json.response.included_rows) > 0);
        }
    }
}