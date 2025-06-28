using GetImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.MyProducts;

namespace WebPage.Pages.Product.Seller {
    public class ViewDetailsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesBuilder _services;
        private readonly GetImageService.GetImageServiceClient _grpcClient;

        [BindProperty (SupportsGet = true)]
        public int id {
            get; set;
        }

        public MyProductDetailsDTO product {
            get; set;
        }


        public ViewDetailsModel (IHttpClientFactory httpClientFactory, ServicesBuilder services, GetImageService.GetImageServiceClient grpcClient) {
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
            string requestURL = $"http://apigateway/api/MyProducts/Details?id={id}";
            ApiResponse<MyProductDetailsDTO>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<MyProductDetailsDTO>> (requestURL);

            if (response?.body != null) {
                product = response.body;
                GetImageReply? grpcResponse = await _grpcClient.GetImageAsync (new GetImageRequest{ ProductId = id });
                product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                product.mimeImage = grpcResponse.ImageType;
            }
        }
    }
}
