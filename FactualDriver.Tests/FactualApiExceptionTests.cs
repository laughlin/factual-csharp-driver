using System.Net;
using FactualDriver.Exceptions;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class FactualApiExceptionTests : FactualIntegrationBase
    {
        [Test]
        public void BadAuthenticatedRequest()
        {
            //Arrange
            var factual = new Factual("badkey", "badsecret");

            //Assert
            var exception = Assert.Throws<FactualApiException>(
                () => factual.Fetch("places",new Query().Field("region").Equal("CA")));

            Assert.AreEqual(exception.StatusCode, HttpStatusCode.Unauthorized);
        }

        [Test]
        public void BadSelectField()
        {
            //Arrange
            var query = new Query().Field("country").Equal("US").Only("hours");

            //Assert
            var exception = Assert.Throws<FactualApiException>(
                () => Factual.Fetch("places", query));

            Assert.AreEqual(HttpStatusCode.BadRequest,exception.StatusCode);
            Assert.IsTrue(exception.Url.StartsWith("t/places"));
            Assert.IsTrue(exception.Response.Contains("select"));
            Assert.IsTrue(exception.Response.Contains("unknown field"));
        }
    }
}