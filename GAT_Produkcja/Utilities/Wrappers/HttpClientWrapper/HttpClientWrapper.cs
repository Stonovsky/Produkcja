using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.Wrappers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient client = new HttpClient();

        public HttpResponseMessage Get(string url)
        {
            return client.GetAsync(url).Result;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await client.GetAsync(url);
        } 

        public async Task<string> GetStringAsync(string url)
        {

            return await client.GetStringAsync(url);
        }

        public HttpResponseMessage Post(string url, HttpContent content)
        {
            return client.PostAsync(url, content).Result;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await client.PostAsync(url, content);
        }
    }
}
