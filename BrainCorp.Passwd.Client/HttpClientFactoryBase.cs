using System.Net.Http;
using System.Net.Http.Headers;


namespace BrainCorp.Passwd.Client
{
    public abstract class HttpClientFactoryBase
    {
        private static object synchRoot = new object();
        private HttpClient httpClient;


        public HttpClient GetHttpClientInstance()
        {
            // Use basic double lock pattern to ensure 
            // class is thread safe.
            if (httpClient == null)
            {
                lock (synchRoot)
                {
                    if (httpClient == null)
                    {
                        httpClient = CreateHttpClient();
                    }
                }
            }

            return httpClient;
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();

            // Add an Accept header for JSON format.
            // Put this as first step in case child class
            // wants to override our default behavior.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Call to child implementation to get Uri for client
            InitializeHttpClient(client);

            return client;
        }

        protected abstract void InitializeHttpClient(HttpClient httpClient);
    }
}
