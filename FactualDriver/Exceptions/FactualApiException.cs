using System;
using System.Net;

namespace FactualDriver.Exceptions
{
    public class FactualApiException : Exception 
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
        public string Url { get; set; }

        public FactualApiException(HttpStatusCode statusCode, string response, string url)
        {
            StatusCode = statusCode;
            Response = response;
            Url = url;
        }
    }
}