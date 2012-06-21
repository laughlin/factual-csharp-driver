using System.Net;
using Google.GData.Client;

namespace FactualDriver.OAuth
{
    /// <summary>
    /// Factual two legged oauth authenticator class, uses google gdata authenticator for proper
    /// oAuth signing. 
    /// </summary>
    public class OAuth2LeggedAuthenticator : OAuthAuthenticator
    {
        /// <summary>
        /// Authenticator constructor.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="consumerKey">oAuth consumer key.</param>
        /// <param name="consumerSecret">oAuth consumer secret key.</param>
        public OAuth2LeggedAuthenticator(string applicationName, string consumerKey, string consumerSecret) : base(applicationName, consumerKey, consumerSecret)
        {
        }

        /// <summary>
        /// Adds authentication headers to the HttpWebRequest
        /// </summary>
        /// <param name="request">HttpWebRequest to add authentication headers.</param>
        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);
            string header = OAuthUtil.GenerateHeader(request.RequestUri, ConsumerKey, ConsumerSecret, null, null, request.Method);
            request.Headers.Add(header);
        }
    }
}