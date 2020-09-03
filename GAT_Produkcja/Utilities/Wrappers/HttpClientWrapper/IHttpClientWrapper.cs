using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.Wrappers
{
    public interface IHttpClientWrapper
    {
        HttpResponseMessage Get(string url);
        HttpResponseMessage Post(string url, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string url);
        Task<string> GetStringAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }
}
