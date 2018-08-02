using System.Net.Http;

namespace BrainCorp.Passwd.Client
{
    public class PasswdClientFactory : HttpClientFactoryBase
    {
        private IWebServiceConfiguration _config;

        public PasswdClientFactory(IWebServiceConfiguration configuration)
        {
            _config = configuration;
        }

        protected override void InitializeHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = _config.Uri;
        }
    }
}
