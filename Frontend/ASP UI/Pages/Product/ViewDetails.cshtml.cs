using ImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;

namespace WebPage.Pages.Product {
    public class ViewDetailsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ImageProductService.ImageProductServiceClient _grpcClient;

        [BindProperty (SupportsGet = true)]
        public int id {
            get; set;
        }

        public ViewDetailsDTO product {
            get; set;
        }


        public ViewDetailsModel (IHttpClientFactory httpClientFactory, IConfiguration config, ImageProductService.ImageProductServiceClient grpcClient) {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _grpcClient = grpcClient;
        }

        public async Task OnGetAsync () {
            product = new ViewDetailsDTO ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            ApiResponse<ViewDetailsDTO>? response = await httpClient.GetFromJsonAsync<ApiResponse<ViewDetailsDTO>> ($"{_config["BackendSettings:BackendUrl"]}/api/Product/ConsultProducts/ViewDetails?id={id}");

            if (response?.body != null) {
                product = response.body;
                ImageProductReply? grpcResponse = await _grpcClient.GetImageAsync (new ImageProductRequest { ProductId = id });
                product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                product.mimeImage = grpcResponse.ImageType;
            }
        }
    }
}
