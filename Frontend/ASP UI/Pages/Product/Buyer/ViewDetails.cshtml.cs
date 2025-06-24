using GetImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;

namespace WebPage.Pages.Product.Buyer {
    public class ViewDetailsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesBuilder _services;
        private readonly GetImageService.GetImageServiceClient _grpcClient;

        public ViewDetailsModel (IHttpClientFactory httpClientFactory, ServicesBuilder services, GetImageService.GetImageServiceClient grpcClient) {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _grpcClient = grpcClient;
        }

        [BindProperty (SupportsGet = true)]
        public int id {
            get; set;
        }

        public ProductDetailsDTO? product {
            get; set;
        }

        public async Task OnGetAsync () {
            product = new ProductDetailsDTO ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            ApiResponse<ProductDetailsDTO>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<ProductDetailsDTO>> ($"{_services.BuyerGetProductDetailsUrl}?Id={id}");

            if (response?.body != null) {
                product = response.body;
                GetImageReply? grpcResponse = await _grpcClient.GetImageAsync (new GetImageRequest { ProductId = id });
                product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                product.mimeImage = grpcResponse.ImageType;
            }
        }
    }
}
