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
        /// <param name="query">Relative query string with factual parameters</param>
        /// <returns></returns>
        public HttpWebRequest CreateWebRequest(string query)
        {
            return CreateWebRequest("GET", query);
        }

        /// <summary>
        /// Create a new Factual WebRequest for granual control  
        /// </summary>
        /// <param name="httpMethod">Http method name, GET, POST, etc</param>
        /// <param name="query">Relative query string with factual parameters</param>
        /// <returns></returns>
        public HttpWebRequest CreateWebRequest(string httpMethod, string query)
        {
            var requestUrl = new Uri(new Uri(FactualApiUrl), query);
            var request = _factualAuthenticator.CreateHttpWebRequest(httpMethod, requestUrl);
            request.Headers.Add("X-Factual-Lib", "factual-dotnet-driver-v0.0.1");
            return request;
        }

        /// <summary>
        /// Execute a query against a factual api with Filter Parameters and return a json string
        /// </summary>
        /// <param name="query">Api address of the request</param>
        /// <param name="filters">List of parameter filters against the api</param>
        /// <returns></returns>
        public string Query(string query, params IFilter[] filters)
        {
            return RawQuery(query, JsonUtil.ToQueryString(filters));
        }

        /// <summary>
        /// Execute a query against a factual api with raw parameters and return a json string
        /// </summary>
        /// <param name="query">Api address of the request</param>
        /// <param name="parameters">Raw query string parameters</param>
        /// <returns></returns>
        public string RawQuery(string query, string parameters)
        {
            var request = CreateWebRequest(string.Format("{0}?{1}", query,parameters));

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