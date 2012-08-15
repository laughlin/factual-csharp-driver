using System.Text;
using System.Linq;
using FactualDriver.Filters;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    [TestFixture]
    public class QueryTests : QueryBase
    {
        [Test]
        public void LimitTest()
        {
            AreEqualQueries("limit=24", new Query().Limit(24));
        }

        [Test]
        public void SearchTest()
        {
            AreEqualQueries("q=Sushi's man Santa Monica", new Query().Search("Sushi's man Santa Monica"));
        }

        [Test]
        public void OnlyTests()
        {
            //Arrange
            var query = new Query().Only("*");
            var multipleSelect = new Query().Only("name", "tel", "category");

            //Assert
            AreEqualQueries("select=*",query);
            AreEqualQueries("select=name,tel,category", multipleSelect);
        }

        [Test]
        public void GeoCirlceTest()
        {
            //Arrange
            var query = new Query().WithIn(new Circle(34.06021, -118.41828, 5000));
            //Assert
            AreEqualQueries("geo={\"$circle\":{\"$center\":[34.06021,-118.41828],\"$meters\":5000}}", query);
        }

        [Test]
        public void TestCommaSeparatedQueries()
        {
            //Arrange
            var query = new Query().SortAsc("name");

            //Assert
            AreEqualQueries("sort=name:asc", query);
            AreEqualQueries("sort=name:asc,country:desc", query.SortDesc("country")); 
        }

        [Test]
        public void LimitPlusSortAscTest()
        {
            //Arrange
            var query = new Query().Limit(10).SortAsc("name");

            //Assert
            AreEqualQueries("limit=10&sort=name:asc", query);
        }

        [Test]
        public void Find20RandomEntitiesSortedAcsByRegionThenByLocalityThenByName()
        {
            //Arrange
            var query = new Query()
                .Limit(20)
                .SortAsc("region")
                .SortAsc("locality")
                .SortDesc("name");

            //Assert
            AreEqualQueries("limit=20&sort=region:asc,locality:asc,name:desc", query);
        }

        [Test]
        public void OffsetTest()
        {
            AreEqualQueries("offset=20",new Query().Offset(20));
        }

        [Test]
        public void LimitAndOffsetPagingTest()
        {
            //Arrange
            AreEqualQueries("limit=10&offset=150", new Query().Limit(10).Offset(150));
        }

        [Test]
        public void IncludeRowCount()
        {
            //Arrange
            var query = new Query().IncludeRowCount(true);

            //Assert
            AreEqualQueries("include_count=true",query);
        }

        [Test]
        public void FieldRegionEqualsToCalifornia()
        {
            //Arrange
            var query = new Query().Field("region").Equal("CA");

            //Assert
            AreEqualQueries("filters={\"region\":{\"$eq\":\"CA\"}}", query);
        }

        [Test]
        public void FeildRegionNotEqualsToCalifornia()
        {
            //Arrange
            var query = new Query().Field("region").NotEqual("CA");

            //Assert
            AreEqualQueries("filters={\"region\":{\"$neq\":\"CA\"}}", query);
        }

        [Test]
        public void FieldRegionEqualsToAny()
        {
            //Arrange
            var query = new Query().Field("region").In("MA","VT","NH","RI","CT");

            //Assert
            AreEqualQueries("filters={\"region\":{\"$in\":[\"MA\",\"VT\",\"NH\",\"RI\",\"CT\"]}}", query);
        }

        [Test]
        public void FieldLocalityDoesNotEqualToAny()
        {
            //Arrange
            var query = new Query().Field("locality").NotIn("Los Angeles","Santa Monica");

            //Assert
            AreEqualQueries("filters={\"locality\":{\"$nin\":[\"Los Angeles\",\"Santa Monica\"]}}", query);
        }

        [Test]
        public void FieldNameBeginsWithTest()
        {
            //Arrange
            var query = new Query().Field("name").BeginsWith("Starbucks");

            //Assert
            AreEqualQueries("filters={\"name\":{\"$bw\":\"Starbucks\"}}", query);
        }

        [Test]
        public void FieldNameDoesNotBeginWithMr()
        {
            //Arrange
            var query = new Query().Field("name").NotBeginsWith("Mr.");

            //Assert
            AreEqualQueries("filters={\"name\":{\"$nbw\":\"Mr.\"}}", query);
        }

        [Test]
        public void FieldNameBeginsWithAny()
        {
            //Arrange
            var query = new Query().Field("name").BeginsWithAny("lt","sg","cpt");

            //Assert
            AreEqualQueries("filters={\"name\":{\"$bwin\":[\"lt\",\"sg\",\"cpt\"]}}", query);
        }


        [Test]
        public void FieldClassDoesNotBeginWithAny()
        {
            //Arrange
            var query = new Query().Field("class").NotBeginsWithAny("beginner", "intermediate");

            //Assert
            AreEqualQueries("filters={\"class\":{\"$nbwin\":[\"beginner\",\"intermediate\"]}}", query);
        }

        [Test]
        public void TestFindRowsWithMissingTelephoneNumbers()
        {
            //Arrange
            var query = new Query().Field("tel").Blank();

            //Assert
            AreEqualQueries("filters={\"tel\":{\"$blank\":true}}", query);
        }

        [Test]
        public void TestFindRowsWithNonBlankTelephoneNumbers()
        {
            //Arrange
            var query = new Query().Field("tel").NotBlank();

            //Assert
            AreEqualQueries("filters={\"tel\":{\"$blank\":false}}", query);
        }

        [Test]
        public void FieldRatingGreaterThan7Point5()
        {
            //Arrange
            var query = new Query().Field("rating").GreaterThan(7.5);

            //Assert
            AreEqualQueries("filters={\"rating\":{\"$gt\":7.5}}", query);
        }

        [Test]
        public void FieldRatingGreaterThanOrEqualThan7Point5()
        {
            //Arrange
            var query = new Query().Field("rating").GreaterThanOrEqual(7.5);

            //Assert
            AreEqualQueries("filters={\"rating\":{\"$gte\":7.5}}", query);
        }

        [Test]
        public void FieldAgeIsLessThan50()
        {
            //Arrange
            var query = new Query().Field("age").LessThan(50);

            //Assert
            AreEqualQueries("filters={\"age\":{\"$lt\":50}}", query);
        }

        [Test]
        public void FieldAgeIsLessThanOrEqualTo50()
        {
            //Arrange
            var query = new Query().Field("age").LessThanOrEqual(50);

            //Assert
            AreEqualQueries("filters={\"age\":{\"$lte\":50}}", query);
        }

        [Test]
        public void FullTestSearchCharlesOnFieldName()
        {
            //Arrange
            var query = new Query().Field("name").Search("Charles");

            //Assert
            AreEqualQueries("filters={\"name\":{\"$search\":\"Charles\"}}", query);
        }

        //Logical operators tests

        [Test]
        public void QueryToFindEntriesWhereNameBeginsWithCoffeeANDTelephoneIsBlank()
        {
            //Arrange
            var query = new Query();
            query.And(query.Field("name").BeginsWith("Coffee"), query.Field("tel").Blank());

            //Assert
            AreEqualQueries("filters={\"$and\":[{\"name\":{\"$bw\":\"Coffee\"}},{\"tel\":{\"$blank\":true}}]}", query);
        }

        [Test]
        public void TestShortHandAddingFilters()
        {
            //Arrange
            Query query = new Query()
                .Field("first_name").Equal("Bradley")
                .Field("region").Equal("CA")
                .Field("locality").Equal("Los Angeles");

            //Assert
            AreEqualQueries("filters={\"$and\":[{\"first_name\":{\"$eq\":\"Bradley\"}},{\"region\":{\"$eq\":\"CA\"}},{\"locality\":{\"$eq\":\"Los Angeles\"}}]}",
            query);
        }

        [Test]
        public void TestOr()
        {
            Query query = new Query().Field("region").In("MA", "VT", "NH");
            query.Or(
                query.Field("first_name").Equal("Chun"),
                query.Field("last_name").Equal("Kok")
            );
            //Assert
            AreEqualQueries("filters={\"$and\":[{\"region\":{\"$in\":[\"MA\",\"VT\",\"NH\"]}},{\"$or\":[{\"first_name\":{\"$eq\":\"Chun\"}},{\"last_name\":{\"$eq\":\"Kok\"}}]}]}", query);
        }


        [Test]
        public void TestNestedFilters()
        {
            //Arrange
            Query query = new Query();
            query.Or(
                query.And(
                    query.Field("city").Equal("Los Angeles"),
                    query.Field("score").Equal("38")
                ),
                query.Or(
                    query.Field("last_name").Equal("Kok"),
                    query.Field("first_name").Equal("Chun")
                )
            );

            //Assert
            AreEqualQueries("filters={\"$or\":[{\"$and\":[{\"city\":{\"$eq\":\"Los Angeles\"}},{\"score\":{\"$eq\":\"38\"}}]},{\"$or\":[{\"last_name\":{\"$eq\":\"Kok\"}},{\"first_name\":{\"$eq\":\"Chun\"}}]}]}",query);
        }

        //[Test]
        //public void TestEncoding()
        //{
        //    var special = "משה";
        //    var test1 = System.Web.HttpUtility.UrlEncode(special, Encoding.UTF8);
        //    byte[] source = Encoding.UTF8.GetBytes(special);
        //    var converted = new string(source.Select(b => (char) b).ToArray());
        //    var result = System.Web.HttpUtility.UrlEncode(converted);

        //    var urlEncodedUtf8 = System.Web.HttpUtility.UrlEncode("משה").ToUpper();
        //    var decoded = Utilities.UrlDecodedValue(urlEncodedUtf8);
        //    var googleEncoded = OAuthBase.EncodingPerRFC3986(decoded);
        //    Assert.AreEqual(urlEncodedUtf8, googleEncoded);
        //}
 
    }
}