using System;
using System.IO;
using System.Net;
using System.Web;
using FactualDriver.Filters;
using FactualDriver.Utils;
using OAuth2LeggedAuthenticator = FactualDriver.OAuth.OAuth2LeggedAuthenticator;

namespace FactualDriver
{
    public class Factual
    {
        private const string FactualApiUrl = "http://api.v3.factual.com";
        private readonly OAuth2LeggedAuthenticator _factualAuthenticator;
        private const string DriverHeaderTag = "factual-dotnet-driver-v1.0.1";

        /// <summary>
        /// Create an instance of Factual .NET driver
        /// </summary>
        /// <param name="oAuthKey">OAuth consumer key</param>
        /// <param name="oAuthSecret">Oauth consumer secret key</param>
        public Factual(string oAuthKey, string oAuthSecret)
        {
            _factualAuthenticator = new OAuth2LeggedAuthenticator("FactualDriver", oAuthKey, oAuthSecret);
        }

        /// <summary>
        /// Create a new Factual HTTP GET WebRequest for granual control  
        /// </summary>
        /// <param name="query">Relative path string with factual parameters</param>
        /// <returns></returns>
        public HttpWebRequest CreateWebRequest(string query)
        {
            return CreateWebRequest("GET", query);
        }

        /// <summary>
        /// Create a new Factual WebRequest for granual control  
        /// </summary>
        /// <param name="httpMethod">Http method name, GET, POST, etc</param>
        /// <param name="query">Relative path string with factual parameters</param>
        /// <returns></returns>
        public HttpWebRequest CreateWebRequest(string httpMethod, string query)
        {
            var requestUrl = new Uri(new Uri(FactualApiUrl), query);
            var request = _factualAuthenticator.CreateHttpWebRequest(httpMethod, requestUrl);
            request.Headers.Add("X-Factual-Lib", DriverHeaderTag);
            return request;
        }

        /// <summary>
        /// Execute a path against a factual api with Filter Parameters and return a json string
        /// </summary>
        /// <param name="query">Api address of the request</param>
        /// <param name="filters">List of parameter filters against the api</param>
        /// <returns></returns>
        public string Query(string query, params IFilter[] filters)
        {
            return RawQuery(query, JsonUtil.ToQueryString(filters));
        }

        public string Fetch(string tableName, Query query)
        {
            return RawQuery(UrlForFetch(tableName),query.ToUrlQuery());
        }

        public string Fetch(string tableName, CrosswalkQuery query)
        {
            return RawQuery(UrlForCrosswalk(tableName), query.ToUrlQuery());
        }

        protected static String UrlForCrosswalk(String tableName)
        {
            return tableName + "/crosswalk";
        }

        protected static String UrlForResolve(String tableName)
        {
            return tableName + "/resolve";
        }

        protected static String UrlForFetch(String tableName)
        {
            return "t/" + tableName;
        }

        protected static String UrlForFacets(String tableName)
        {
            return "t/" + tableName + "/facets";
        }

        protected static String UrlForGeocode()
        {
            return "places/geocode";
        }

        protected static String UrlForGeopulse()
        {
            return "places/geopulse";
        }

        /// <summary>
        /// Execute a path against a factual api with raw parameters and return a json string
        /// </summary>
        /// <param name="path">Api address of the request</param>
        /// <param name="queryParameters">Raw path string parameters</param>
        /// <returns></returns>
        public string RawQuery(string path, string queryParameters)
        {
            var request = CreateWebRequest(string.Format("{0}?{1}", path,queryParameters));

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var jsonResult = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(jsonResult))
                            throw new InvalidOperationException("No data received from factual");

                        return jsonResult;
                    }
                }
            }
            catch (WebException ex)
            {
                var response = ((HttpWebResponse)ex.Response);

                try
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var text = reader.ReadToEnd();
                            return text;
                        }
                    }
                }
                catch (WebException e)
                {
                    return response.StatusCode.ToString() + e.Message;
                }
            }

        }
    }
}