using ImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.MyProducts;

namespace WebPage.Pages.Product.Seller {
    public class ViewDetailsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesBuilder _services;
        private readonly ImageProductService.ImageProductServiceClient _grpcClient;

        [BindProperty (SupportsGet = true)]
        public int id {
            get; set;
        }

        public MyProductDetailsDTO product {
            get; set;
        }


        public ViewDetailsModel (IHttpClientFactory httpClientFactory, ServicesBuilder services, ImageProductService.ImageProductServiceClient grpcClient) {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _grpcClient = grpcClient;
        }

        public async Task OnGetAsync () {
            product = new MyProductDetailsDTO ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();
            if (!string.IsNullOrEmpty (cookies))
                httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);
            ApiResponse<MyProductDetailsDTO>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<MyProductDetailsDTO>> ($"{_services.SellerGetProductDetailsUrl}?id={id}");

            if (response?.body != null) {
                product = response.body;
                ImageProductReply? grpcResponse = await _grpcClient.GetImageAsync (new ImageProductRequest { ProductId = id });
                product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                product.mimeImage = grpcResponse.ImageType;
            }
        }
    }
}
