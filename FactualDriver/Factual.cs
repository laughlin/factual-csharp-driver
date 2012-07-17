using System;
using System.IO;
using System.Net;
using System.Web;
using FactualDriver.Exceptions;
using FactualDriver.Filters;
using FactualDriver.Utils;
using OAuth2LeggedAuthenticator = FactualDriver.OAuth.OAuth2LeggedAuthenticator;

namespace FactualDriver
{
    /// <summary>
    /// Main point of entry for the driver. Supports running queries against Factual
    /// and inspecting the response. Supports the same levels of authentication
    /// supported by Factual's API.
    /// </summary>
    public class Factual
    {
        private const string FactualApiUrl = "http://api.v3.factual.com";
        private readonly OAuth2LeggedAuthenticator _factualAuthenticator;
        private const string DriverHeaderTag = "factual-dotnet-driver-v1.2.0";
        private MultiQuery _multiQuery;

        /// <summary>
        /// MultiQuery accessor. Creates and returns new instance of MultiQuery if one already doesn't exists.
        /// </summary>
        public MultiQuery MultiQuery
        {
            get { return _multiQuery ?? (_multiQuery = new MultiQuery()); }
        }

        /// <summary>
        /// Set the driver in or out of debug mode. True to display in the output window.
        /// </summary>
        public bool Debug { get; set; }

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
        /// Create an instance of Factual .NET driver
        /// </summary>
        /// <param name="oAuthKey">OAuth consumer key</param>
        /// <param name="oAuthSecret">Oauth consumer secret key</param>
        /// <param name="debug">Include debuggin info</param>
        public Factual(string oAuthKey, string oAuthSecret, bool debug)
        {
            _factualAuthenticator = new OAuth2LeggedAuthenticator("FactualDriver", oAuthKey, oAuthSecret);
            Debug = debug;
        }

        /// <summary>
        /// Create a new Factual HTTP GET WebRequest for granual control  
        /// </summary>
        /// <param name="fullQuery">Relative path string with factual query parameters</param>
        /// <returns></returns>
        public HttpWebRequest CreateWebRequest(string fullQuery)
        {
            return CreateWebRequest("GET", fullQuery);
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

        /// <summary>
        /// Runs a read query against the specified Factual table.
        /// </summary>
        /// <param name="tableName">the name of the table you wish to query (e.g., "places")</param>
        /// <param name="query">the read query to run against table.</param>
        /// <returns>the response of running query against Factual.</returns>
        public string Fetch(string tableName, Query query)
        {
            return RawQuery(UrlForFetch(tableName),query.ToUrlQuery());
        }

        /// <summary>
        /// Runs a Crosswalk query against the specified Factual table.
        /// </summary>
        /// <param name="tableName">the name of the table you wish to query (e.g., "places")</param>
        /// <param name="query">the Crosswalk query to run against table.</param>
        /// <returns>the response of running query against Factual.</returns>
        public string Fetch(string tableName, CrosswalkQuery query)
        {
            return RawQuery(UrlForCrosswalk(tableName), query.ToUrlQuery());
        }

        /// <summary>
        /// Asks Factual to resolve the entity for the attributes specified by
        /// query, within the table called tableName.
        /// Returns the read response from a Factual Resolve request, which includes
        /// all records that are potential matches.
        /// Each result record will include a confidence score ("similarity"),
        /// and a flag indicating whether Factual decided the entity is the correct
        /// resolved match with a high degree of accuracy ("resolved").
        /// There will be 0 or 1 entities returned with "resolved"=true. If there was a
        /// full match, it is guaranteed to be the first record in the response.
        /// </summary>
        /// <param name="tableName">the name of the table to resolve within.</param>
        /// <param name="query">a Resolve query with partial attributes for an entity.</param>
        /// <returns>the response from Factual for the Resolve request.</returns>
        public string Fetch(string tableName, ResolveQuery query)
        {
            return RawQuery(UrlForResolve(tableName), query.ToUrlQuery());
        }

        /// <summary>
        /// Runs a facet read against the specified Factual table.
        /// </summary>
        /// <param name="tableName">the name of the table you wish to query for facets (e.g., "places")</param>
        /// <param name="query">the facet query to run against table</param>
        /// <returns>the response of running facet against Factual.</returns>
        public string Fetch(string tableName, FacetQuery query)
        {
            return RawQuery(UrlForFacets(tableName), query.ToUrlQuery());
        }

        /// <summary>
        /// Run a schema query against the specified Factual table.
        /// </summary>
        /// <param name="tableName">the name of the table you wish to query (e.g., "places")</param>
        /// <returns>the response of running query against Factual.</returns>
        public string Schema(string tableName)
        {
            return RawQuery(UrlForSchema(tableName));
        }

        /// <summary>
        /// Run a Geopulse query against the Factual API.
        /// <para name="Example"> This example shows common usage: </para>
        /// <c>new Geopulse(new Point(34.06021, -118.41828)).Only("commercial_density")</c>
        /// </summary>
        /// <param name="geopulse">Geopulse query to run</param>
        /// <returns>the response of running query against Factual.</returns>
        public string Geopulse(Geopulse geopulse)
        {
            return RawQuery(UrlForGeopulse(), geopulse.ToUrlQuery());
        }

        /// <summary>
        /// Run a Geocode query against the Factual API.
        /// <para name="Example"> This example shows common usage: </para>
        /// <c>new Point(34.06021, -118.41828)</c>
        /// </summary>
        /// <param name="point">Point to get geocode</param>
        /// <returns>the response of running query against Factual.</returns>
        public string ReverseGeocode(Point point)
        {
            return RawQuery(UrlForGeocode(), point.ToUrlQuery());
        }


        /// <summary>
        /// Queue a raw read request for inclusion in the next multi request.
        /// </summary>
        /// <param name="path">the path to run the request against</param>
        /// <param name="query">the parameters to send with the request</param>
        public void QueueFetch(string path, string query)
        {
            MultiQuery.AddQuery(path, query);
        }

        /// <summary>
        /// Queue a read request for inclusion in the next multi request.
        /// </summary>
        /// <param name="table">the name of the table you wish to query (e.g., "places")</param>
        /// <param name="query">the read query to run against table.</param>
        public void QueueFetch(string table, Query query)
        {
            MultiQuery.AddQuery(UrlForFetch(table), query.ToUrlQuery());
        }

        /// <summary>
        /// Queue a crosswalk request for inclusion in the next multi request.
        /// </summary>
        /// <param name="table">the name of the table you wish to use crosswalk against (e.g., "places")</param>
        /// <param name="query">the crosswalk query to run against table</param>
        public void QueueFetch(string table, CrosswalkQuery query)
        {
            MultiQuery.AddQuery(UrlForCrosswalk(table), query.ToUrlQuery());
        }

        /// <summary>
        /// Queue a resolve request for inclusion in the next multi request.
        /// </summary>
        /// <param name="table">the name of the table you wish to use resolve against (e.g., "places")</param>
        /// <param name="query">the resolve query to run against table.</param>
        public void QueueFetch(string table, ResolveQuery query)
        {
            MultiQuery.AddQuery(UrlForResolve(table), query.ToUrlQuery());
        }

        /// <summary>
        /// Queue a facet request for inclusion in the next multi request.
        /// </summary>
        /// <param name="table">the name of the table you wish to use a facet request against (e.g., "places")</param>
        /// <param name="query">the facet query to run against table.</param>
        public void QueueFetch(string table, FacetQuery query)
        {
            MultiQuery.AddQuery(UrlForFacets(table), query.ToUrlQuery());
        }

        /// <summary>
        /// Queue a ReverseGeocode for inclusion in the next multi request.
        /// </summary>
        /// <param name="point">the geo location point parameter</param>
        public void QueueFetch(Point point)
        {
            MultiQuery.AddQuery(UrlForGeocode(), point.ToUrlQuery());
        }

        /// <summary>
        /// Queue a Geopulse for inclusion in the next multi request.
        /// </summary>
        /// <param name="point">Geopulse query parameter</param>
        public void QueueFetch(Geopulse point)
        {
            MultiQuery.AddQuery(UrlForGeopulse(), point.ToUrlQuery());
        }

        /// <summary>
        /// Send all milti query requests which were queued up.
        /// </summary>
        /// <returns></returns>
        public string SendQueueRequests()
        {
            return RawQuery(UrlForMulti(), MultiQuery.ToUrlQuery());
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

        protected static string UrlForSchema(string tableName)
        {
            return "t/" + tableName + "/schema";
        }

        protected static string UrlForMulti()
        {
            return "multi";
        }

        /// <summary>
        /// Execute a path against a factual api with raw path and query parameters and return a json string
        /// </summary>
        /// <param name="path">Api address of the request</param>
        /// <param name="queryParameters">Raw path string parameters</param>
        /// <returns></returns>
        public string RawQuery(string path, string queryParameters)
        {
            return RawQuery(string.Format("{0}?{1}", path, queryParameters));
        }

        /// <summary>
        /// Execute a raw query again a factual api.
        /// </summary>
        /// <param name="completePathWithQuery">Complete path and query excluding api uri host</param>
        /// <returns></returns>
        public string RawQuery(string completePathWithQuery)
        {
            var request = CreateWebRequest(completePathWithQuery);
            if(Debug)
            {
                System.Diagnostics.Debug.WriteLine("==== Request Url =====");
                System.Diagnostics.Debug.WriteLine(request.RequestUri);
            }
                
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if(stream == null)
                            throw new FactualException("Did not receive a response stream from factual");

                        using (var reader = new StreamReader(stream))
                        {
                            var jsonResult = reader.ReadToEnd();
                            if (string.IsNullOrEmpty(jsonResult))
                                throw new FactualException("No data received from factual");

                            if (Debug)
                            {
                                System.Diagnostics.Debug.WriteLine("===== Factual Response =====");
                                System.Diagnostics.Debug.WriteLine(jsonResult);
                            }

                            return jsonResult;
                        }
                    }

                }
            }
            catch (WebException ex)
            {
                var response = ((HttpWebResponse)ex.Response);
                using (var stream = response.GetResponseStream())
                {
                    if(stream == null)
                        throw new FactualApiException(response.StatusCode, string.Empty, completePathWithQuery);

                    using (var reader = new StreamReader(stream))
                    {
                        var text = reader.ReadToEnd();

                        if (Debug)
                        {
                            System.Diagnostics.Debug.WriteLine("===== Factual API Error =====");
                            System.Diagnostics.Debug.WriteLine(text);
                        }

                        throw new FactualApiException(response.StatusCode, text, HttpUtility.UrlDecode(completePathWithQuery));
                    }
                }
            }
        }
    }
}