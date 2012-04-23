using System.Net;
using Google.GData.Client;

namespace FactualDriver.OAuth
{
    public class OAuth2LeggedAuthenticator : OAuthAuthenticator
    {
        public OAuth2LeggedAuthenticator(string applicationName, string consumerKey, string consumerSecret) : base(applicationName, consumerKey, consumerSecret)
        {
        }

        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);
            string header = OAuthUtil.GenerateHeader(request.RequestUri, ConsumerKey, ConsumerSecret, null, null, request.Method);
            request.Headers.Add(header);
        }
    }
}