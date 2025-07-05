using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Utilities
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<HttpResponseMessage> PostAsync<T>(string url, T data, string? token = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(data)
            };

            AddAuthorizationHeader(request, token);

            return await _client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PatchAsync<T>(string url, T data, string? token = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(data)
            };

            AddAuthorizationHeader(request, token);

            return await _client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> GetAsync(string url, string? token = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            AddAuthorizationHeader(request, token);

            return await _client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string url, string? token = null)
        {
            using var client = new HttpClient();

            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            return await client.SendAsync(request);
        }


        private static void AddAuthorizationHeader(HttpRequestMessage request, string? token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }

}
