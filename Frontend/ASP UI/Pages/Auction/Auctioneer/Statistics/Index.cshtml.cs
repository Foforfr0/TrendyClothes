using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Auction;

namespace WebPage.Pages.Auction.Auctioneer.Statistics {
    public class IndexModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;

        public IndexModel (IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> servicesBuilder) {
            _httpClientFactory = httpClientFactory;
            _services = servicesBuilder.Value;
        }

        public List<StatisticsAuctionDTO> StatisticsAuctions { get; set; } = new List<StatisticsAuctionDTO> ();

        public async Task OnGetAsync () {
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();
            string? username = User.Identity?.Name ?? "";

            HttpClient? clientGetAuction = _httpClientFactory.CreateClient ();
            if (!string.IsNullOrEmpty (cookies))
                clientGetAuction.DefaultRequestHeaders.Add ("Cookie", cookies);
            HttpResponseMessage? response = await clientGetAuction.GetAsync ($"http://apigateway{_services.REST.Auction.Auctioneer.GetAuctions}?username={username}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return;
            if (response.StatusCode != HttpStatusCode.OK)
                return;

            ApiResponse<List<MyAuctionsDTO>>? auctionsResult = await response.Content.ReadFromJsonAsync<ApiResponse<List<MyAuctionsDTO>>> ();
            if (auctionsResult?.body == null)
                return;

            ApiResponse<StatisticsAuctionDTO>? responseStatistic;
            foreach (MyAuctionsDTO auction in auctionsResult.body) {
                HttpClient? clientGetStatistics = _httpClientFactory.CreateClient ();
                if (!string.IsNullOrEmpty (cookies))
                    clientGetStatistics.DefaultRequestHeaders.Add ("Cookie", cookies);
                HttpResponseMessage? responseStatisitcs = await clientGetAuction.GetAsync ($"http://apigateway{_services.REST.Auction.Statistics.GetStatisticsAuction}?idAuction={auction.Id}");
                if (response.StatusCode == HttpStatusCode.NotFound)
                    continue;
                responseStatistic = await responseStatisitcs.Content.ReadFromJsonAsync<ApiResponse<StatisticsAuctionDTO>> ();

               StatisticsAuctionDTO aux = new StatisticsAuctionDTO {
                    IdAuction = responseStatistic.body.IdAuction,
                    NumberBids = responseStatistic.body.NumberBids,
                    FirstPrice = responseStatistic.body.FirstPrice,
                    LastPrice = responseStatistic.body.LastPrice,
                    PercentageGain = responseStatistic.body.PercentageGain,
                    Name = auction.Name,
                    DateStart = auction.DateStart,
                    DateEnd = auction.DateEnd,
                    Status = auction.Status,
                    ImageBase64 = auction.ImageBase64,
                    MimeImage = auction.MimeImage,
                };
                StatisticsAuctions.Add (aux);
            }
        }
    }
}
