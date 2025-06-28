using GetImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Auction;
using WebPage.DTO.Product.MyProducts;

namespace WebPage.Pages.Auction.Auctioneer {
    public class CreateAuctionModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesBuilder _services;
        private readonly GetImageService.GetImageServiceClient _grpcClient;

        public CreateAuctionModel (IHttpClientFactory httpClientFactory, ServicesBuilder services, GetImageService.GetImageServiceClient grpcClient) {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _grpcClient = grpcClient;
        }

        [BindProperty (SupportsGet = true)]
        public int idProduct {
            get; set;
        }

        public MyProductDetailsDTO product {
            get; set;
        }

        [BindProperty]
        public CreateAuctionDTO newAuction {
            get; set;
        }

        public async Task OnGetAsync () {
            product = new MyProductDetailsDTO ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();
            if (!string.IsNullOrEmpty (cookies))
                httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);
            string requestURL = $"http://apigateway/api/MyProducts/Search?id={this.idProduct}";
            ApiResponse<MyProductDetailsDTO>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<MyProductDetailsDTO>> (requestURL);

            if (response?.body != null) {
                product = response.body;
                GetImageReply? grpcResponse = await _grpcClient.GetImageAsync (new GetImageRequest { ProductId = this.idProduct });
                product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                product.mimeImage = grpcResponse.ImageType;
            }
        }
    }
}
