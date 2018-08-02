using System;
using System.Net.Http;

namespace BrainCorp.Passwd.Client
{
    public class PasswdClientBase
    {
        PasswdClientFactory _httpClientFactory;

        public PasswdClientBase(
            PasswdClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient GetHttpClient()
        {
            return _httpClientFactory.GetHttpClientInstance();
        }

        protected void ToApplicationException(HttpResponseMessage response)
        {
            throw new ApplicationException(string.Format("Call failed to BrainCorp.Passwd @ {0}.  Failure Reason: {1} - {2}"
                , response.RequestMessage.RequestUri, response.StatusCode, response.Content.ReadAsStringAsync().Result));
        }
    }
}
