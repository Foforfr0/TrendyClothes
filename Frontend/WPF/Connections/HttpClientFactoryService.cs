using System.Net.Http;

namespace WpfApp.Connections {
    public class HttpClientFactoryService {
        private readonly IHttpClientFactory _factory;

        public HttpClientFactoryService (IHttpClientFactory factory) {
            _factory = factory;
        }

        public HttpClient GetClient (string name) {
            return _factory.CreateClient (name);
        }
    }
}
