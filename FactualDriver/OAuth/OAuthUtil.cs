/* Copyright (c) 2011 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * 
*/

using System;
using System.Text;

namespace FactualDriver.OAuth
{
    /// <summary>
    /// Provides a means to generate an OAuth signature.
    /// </summary>
    public class OAuthUtil
    {

        /// <summary>
        /// Generates an OAuth header.
        /// </summary>
        /// <param name="uri">The URI of the request</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer secret</param>
        /// <param name="httpMethod">The http method</param>
        /// <returns>The OAuth authorization header</returns>
        public static string GenerateHeader(Uri uri, string consumerKey, string consumerSecret, string httpMethod)
        {
            return GenerateHeader(uri, consumerKey, consumerSecret, string.Empty, string.Empty, httpMethod);
        }

        /// <summary>
        /// Generates an OAuth header.
        /// </summary>
        /// <param name="uri">The URI of the request</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer secret</param>
        /// <param name="token">The OAuth token</param>
        /// <param name="tokenSecret">The OAuth token secret</param>
        /// <param name="httpMethod">The http method</param>
        /// <returns>The OAuth authorization header</returns>
        public static string GenerateHeader(Uri uri, string consumerKey, string consumerSecret, string token,
            string tokenSecret, string httpMethod)
        {
            OAuthParameters parameters = new OAuthParameters()
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Token = token,
                TokenSecret = tokenSecret,
                SignatureMethod = OAuthBase.HMACSHA1SignatureType
            };
            return GenerateHeader(uri, httpMethod, parameters);
        }

        /// <summary>
        /// Generates an OAuth header.
        /// </summary>
        /// <param name="uri">The URI of the request</param>
        /// <param name="httpMethod">The http method</param>
        /// <param name="parameters">The OAuth parameters</param>
        /// <returns>The OAuth authorization header</returns>
        public static string GenerateHeader(Uri uri, string httpMethod, OAuthParameters parameters)
        {
            parameters.Timestamp = OAuthBase.GenerateTimeStamp();
            parameters.Nonce = OAuthBase.GenerateNonce();

            string signature = OAuthBase.GenerateSignature(uri, httpMethod, parameters);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Authorization: OAuth {0}=\"{1}\",", OAuthBase.OAuthVersionKey, OAuthBase.OAuthVersion);
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthNonceKey, OAuthBase.EncodingPerRFC3986(parameters.Nonce));
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthTimestampKey, OAuthBase.EncodingPerRFC3986(parameters.Timestamp));
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthConsumerKeyKey, OAuthBase.EncodingPerRFC3986(parameters.ConsumerKey));
            if (parameters.BaseProperties.ContainsKey(OAuthBase.OAuthVerifierKey))
            {
                sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthVerifierKey, OAuthBase.EncodingPerRFC3986(parameters.BaseProperties[OAuthBase.OAuthVerifierKey]));
            }
            if (!String.IsNullOrEmpty(parameters.Token))
            {
                sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthTokenKey, OAuthBase.EncodingPerRFC3986(parameters.Token));
            }
            if (parameters.BaseProperties.ContainsKey(OAuthBase.OAuthCallbackKey))
            {
                sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthCallbackKey, OAuthBase.EncodingPerRFC3986(parameters.BaseProperties[OAuthBase.OAuthCallbackKey]));
            }
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthSignatureMethodKey, OAuthBase.HMACSHA1SignatureType);
            sb.AppendFormat("{0}=\"{1}\"", OAuthBase.OAuthSignatureKey, OAuthBase.EncodingPerRFC3986(signature));

            return sb.ToString();
        }
    }
}
