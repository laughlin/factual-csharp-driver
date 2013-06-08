using System.Configuration;
using NUnit.Framework;

namespace FactualDriver.Tests
{
    public class FactualIntegrationBase
    {
        private static readonly string OAuthKey = ConfigurationManager.AppSettings["oAuthKey"];
        private static readonly string OAuthSecret = ConfigurationManager.AppSettings["oAuthSecret"];
        public Factual Factual { get; set; }

        [SetUp]
        public void Init()
        {
            if (string.IsNullOrWhiteSpace(OAuthKey) || string.IsNullOrEmpty(OAuthSecret))
                throw new ConfigurationErrorsException("please specify oauth keys");

            Factual = new Factual(OAuthKey, OAuthSecret);
            Factual.Debug = true;
        }
    }
}