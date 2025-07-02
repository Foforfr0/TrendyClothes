using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Utilities
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            return await _client.PostAsJsonAsync(url, data);
        }

        public static async Task<HttpResponseMessage> PatchAsync<T>(string url, T data)
        {
            var content = JsonContent.Create(data);
            var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
            return await _client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _client.DeleteAsync(url);
        }
    }
}
