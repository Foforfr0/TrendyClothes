using GetImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;
using Microsoft.Extensions.Options;

namespace WebPage.Pages.Product.Buyer {
    public class ViewDetailsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;
        private readonly GetImageService.GetImageServiceClient _grpcClient;

        public ViewDetailsModel (IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> services, GetImageService.GetImageServiceClient grpcClient) {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
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
            string requestURL = $"http://apigateway{_services.REST.Product.Buyer.GetDetailsProduct}?Id={id}";
            ApiResponse<ProductDetailsDTO>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<ProductDetailsDTO>> (requestURL);

            if (response?.body != null) {
                product = response.body;
                GetImageReply? grpcResponse = await _grpcClient.GetImageAsync (new GetImageRequest { ProductId = id });
                product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                product.mimeImage = grpcResponse.ImageType;
            }
        }
    }
}
