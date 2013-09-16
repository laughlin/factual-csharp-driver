using System;
using System.Net;

namespace FactualDriver.OAuth
{
    /// <summary>
    /// Factual two legged oauth authenticator class
    /// oAuth signing. 
    /// </summary>
    public class OAuth2LeggedAuthenticator
    {
        private string _constumerKey;
        private string _constumerSecret;

        /// <summary>
        ///Authenticator constructor.
        /// </summary>
        /// <param name="consumerKey">oAuth consumer key.</param>
        /// <param name="consumerSecret">oAuth consumer secret key.</param>
        public OAuth2LeggedAuthenticator(string consumerKey, string consumerSecret)
        {
            _constumerKey = consumerKey;
            _constumerSecret = consumerSecret;
        }

        /// <summary>
        ///Adds authentication headers to the HttpWebRequest
        /// </summary>
        /// <param name="request">HttpWebRequest to add authentication headers.</param>
        public void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            string header = OAuthUtil.GenerateHeader(request.RequestUri, _constumerKey, _constumerSecret, null, null, request.Method);
            request.Headers.Add(header);
        }

        public HttpWebRequest CreateHttpWebRequest(string httpMethod, Uri targetUri)
        {
            HttpWebRequest request = WebRequest.Create(targetUri) as HttpWebRequest;
            request.AllowAutoRedirect = false;
            request.Method = httpMethod;
            ApplyAuthenticationToRequest(request);
            return request;
        }
    }
}