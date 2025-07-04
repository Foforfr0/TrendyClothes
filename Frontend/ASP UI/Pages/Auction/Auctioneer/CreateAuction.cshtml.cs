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

        [BindProperty]
        public CreateAuctionDTO newAuction {
            get; set;
        }

        public async Task OnGetAsync () {
        }
    }
}
