using ImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;

namespace WebPage.Pages.Product {
    public class ConsultProductsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ImageProductService.ImageProductServiceClient _grpcClient;

        [BindProperty (SupportsGet = true)]
        public string query {
            get; set;
        }

        public List<SearchProductsDTO> Products {
            get; set;
        }

        public ConsultProductsModel (IHttpClientFactory httpClientFactory, IConfiguration config, ImageProductService.ImageProductServiceClient grpcClient) {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _grpcClient = grpcClient;
        }

        public async Task OnGetAsync () {
            if (string.IsNullOrWhiteSpace (query)) {
                Products = new List<SearchProductsDTO> ();
                return;
            }

            HttpClient httpClient = _httpClientFactory.CreateClient ();
            string requestURL = $"{_config["BackendSettings:BackendUrl"]}/api/Product/ConsultProducts/Search?query={query}";
            HttpResponseMessage response = await httpClient.GetAsync (requestURL);

            HttpStatusCode statusCode = response.StatusCode;

            ApiResponse<List<SearchProductsDTO>>? responseData = new ApiResponse<List<SearchProductsDTO>> ();

            if (statusCode == HttpStatusCode.NotFound) {
                responseData = new ApiResponse<List<SearchProductsDTO>> ();
                return;
            }

            if (statusCode == HttpStatusCode.OK)
                responseData = await response.Content.ReadFromJsonAsync<ApiResponse<List<SearchProductsDTO>>> ();

            if (responseData?.body != null)
                Products = new List<SearchProductsDTO> ();
            foreach (SearchProductsDTO prod in responseData.body) {
                ImageProductReply grpcResponse = await _grpcClient.GetImageAsync (new ImageProductRequest { ProductId = prod.id });

                SearchProductsDTO aux = new SearchProductsDTO {
                    id = prod.id,
                    name = prod.name,
                    price = prod.price,
                    discount = prod.discount,
                    numberSold = prod.numberSold,
                    averageStars = prod.averageStars,
                    stockAvailable = prod.stockAvailable,
                    category = prod.category,
                    type = prod.type
                };
                aux.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                aux.mimeImage = grpcResponse.ImageType;

                Products.Add (aux);
            }
        }
    }
}