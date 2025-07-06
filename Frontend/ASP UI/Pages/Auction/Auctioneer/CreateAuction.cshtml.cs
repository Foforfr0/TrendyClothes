using GetImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Auction;

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
        } = new CreateAuctionDTO ();

        public List<StatusAuctionDTO>? statuses;
        public SelectList? statusSelectList;

        public async Task OnGetAsync () {
            await InitializeStatussesList ();
        }

        public async Task InitializeStatussesList () {
            statuses = new List<StatusAuctionDTO> ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string requestURL = "http://apigateway/api/Auction/Statuses";
            ApiResponse<List<StatusAuctionDTO>>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<List<StatusAuctionDTO>>> (requestURL);

            if (response?.body != null) {
                statuses = response.body;
                statusSelectList = new SelectList (statuses, "Id", "Status");
            }
        }
    }
}
