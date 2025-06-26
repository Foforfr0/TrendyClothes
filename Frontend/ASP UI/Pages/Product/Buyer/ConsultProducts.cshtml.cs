using GetImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;

namespace WebPage.Pages.Product.Buyer {
    public class ConsultProductsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ConsultProductsModel> _logger;
        private readonly ServicesBuilder _services;
        private readonly GetImageService.GetImageServiceClient _grpcClient;

        [BindProperty (SupportsGet = true)]
        public string query {
            get; set;
        }

        public List<ProductsDTO> Products {
            get; set;
        }

        public ConsultProductsModel (IHttpClientFactory httpClientFactory, ServicesBuilder services, GetImageService.GetImageServiceClient grpcClient, ILogger<ConsultProductsModel> logger) {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _grpcClient = grpcClient;
            _logger = logger;
        }

        public async Task OnGetAsync () {
            if (string.IsNullOrWhiteSpace (query)) {
                Products = new List<ProductsDTO> ();
                return;
            }

            HttpClient httpClient = _httpClientFactory.CreateClient ();
            string requestURL = $"http://productbuyerservice/api/Product/Search?query={query}";
            _logger.LogInformation ("ConsultProductsModel.OnGetAsync: " + requestURL);
            HttpResponseMessage response = await httpClient.GetAsync (requestURL);

            HttpStatusCode statusCode = response.StatusCode;

            ApiResponse<List<ProductsDTO>>? responseData = new ApiResponse<List<ProductsDTO>> ();

            if (statusCode == HttpStatusCode.NotFound) {
                responseData = new ApiResponse<List<ProductsDTO>> ();
                return;
            }

            if (statusCode == HttpStatusCode.OK)
                responseData = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductsDTO>>> ();

            if (responseData?.body != null)
                Products = new List<ProductsDTO> ();
            foreach (ProductsDTO prod in responseData.body) {
                ProductsDTO aux = new ProductsDTO {
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
                GetImageReply grpcResponse = await _grpcClient.GetImageAsync (new GetImageRequest { ProductId = prod.id });
                aux.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                aux.mimeImage = grpcResponse.ImageType;

                Products.Add (aux);
            }
        }
    }
}