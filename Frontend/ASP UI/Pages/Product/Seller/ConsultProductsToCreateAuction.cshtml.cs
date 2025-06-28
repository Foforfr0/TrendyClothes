using GetImageProduct;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.MyProducts;

namespace WebPage.Pages.Product.Seller {
    public class ConsultProductsToCreateAuctionModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesBuilder _services;
        private readonly GetImageService.GetImageServiceClient _grpcClient;

        public ConsultProductsToCreateAuctionModel (
                IHttpClientFactory httpClientFactory, ServicesBuilder servicesBuilder,
                GetImageService.GetImageServiceClient grpcClient) {
            _httpClientFactory = httpClientFactory;
            _services = servicesBuilder;
            _grpcClient = grpcClient;
        }

        private string? username {
            get; set;
        }

        public List<MyProductsDTO> Products {
            get; set;
        }

        public async Task OnGetAsync () {
            this.username = HttpContext?.User.Identity?.Name ?? string.Empty;

            string requestURL = $"http://apigateway/api/MyProducts/Search?username={username}";
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();

            HttpClient httpClient = _httpClientFactory.CreateClient ();

            if (!string.IsNullOrEmpty (cookies))
                httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);
            HttpResponseMessage response = await httpClient.GetAsync (requestURL);

            HttpStatusCode statusCode = response.StatusCode;

            ApiResponse<List<MyProductsDTO>>? responseData = new ApiResponse<List<MyProductsDTO>> ();

            if (statusCode == HttpStatusCode.NotFound) {
                responseData = new ApiResponse<List<MyProductsDTO>> ();
                return;
            }

            if (statusCode == HttpStatusCode.OK)
                responseData = await response.Content.ReadFromJsonAsync<ApiResponse<List<MyProductsDTO>>> ();

            if (responseData?.body != null)
                Products = new List<MyProductsDTO> ();
            foreach (MyProductsDTO prod in responseData.body) {
                GetImageReply grpcResponse = await _grpcClient.GetImageAsync (new GetImageRequest { ProductId = prod.id });

                MyProductsDTO aux = new MyProductsDTO {
                    id = prod.id,
                    name = prod.name,
                    price = prod.price,
                    discount = prod.discount,
                    numberSold = prod.numberSold,
                    averageStars = prod.averageStars,
                    stockAvailable = prod.stockAvailable,
                    category = prod.category,
                    type = prod.type,
                    status = prod.status
                };
                aux.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                aux.mimeImage = grpcResponse.ImageType;

                Products.Add (aux);
            }
        }
    }
}
