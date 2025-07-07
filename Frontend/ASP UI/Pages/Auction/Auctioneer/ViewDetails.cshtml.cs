using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Auction;

namespace WebPage.Pages.Auction.Auctioneer {
    public class ViewDetailsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;

        [BindProperty (SupportsGet = true)]
        public int id {
            get; set;
        }

        public MyAuctionDetailsDTO auction {
            get; set;
        }

        public ViewDetailsModel (IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> services) {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
        }

        public async Task OnGetAsync () {
            auction = new MyAuctionDetailsDTO ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();
            if (!string.IsNullOrEmpty (cookies))
                httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);
            string requestURL = $"http://apigateway{_services.REST.Auction.Auctioneer.GetDetailsAuction}?id={id}";
            ApiResponse<MyAuctionDetailsDTO>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<MyAuctionDetailsDTO>> (requestURL);

            if (response?.body != null) {
                auction = response.body;
            }
        }
    }
}
