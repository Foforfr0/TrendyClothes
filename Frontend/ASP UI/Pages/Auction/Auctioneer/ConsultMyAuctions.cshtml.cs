using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Auction;
using Microsoft.Extensions.Options;

namespace WebPage.Pages.Auction.Auctioneer {
    public class ConsultMyAuctionsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;
        private readonly ILogger<ConsultMyAuctionsModel> _logger;

        public List<MyAuctionsDTO> Auctions {
            get; set;
        }

        public ConsultMyAuctionsModel (IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> servicesBuilder, ILogger<ConsultMyAuctionsModel> logger) {
            _httpClientFactory = httpClientFactory;
            _services = servicesBuilder.Value;
            _logger = logger;
        }

        public async Task OnGetAsync () {
            string requestUrl = $"http://apigateway{_services.REST.Auction.Auctioneer.GetAuctions}?username={User.Identity?.Name??""}";
            _logger.LogInformation ("ConsultMyAuctionsModel.OnGetAsync: " + requestUrl);
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();

            HttpClient httpClient = _httpClientFactory.CreateClient ();

            if (!string.IsNullOrEmpty (cookies))
                httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);
            HttpResponseMessage response = await httpClient.GetAsync (requestUrl);

            HttpStatusCode statusCode = response.StatusCode;

            ApiResponse<List<MyAuctionsDTO>>? responseData = new ApiResponse<List<MyAuctionsDTO>> ();

            if (statusCode == HttpStatusCode.NotFound) {
                responseData = new ApiResponse<List<MyAuctionsDTO>> ();
                return;
            }

            if (statusCode == HttpStatusCode.OK)
                responseData = await response.Content.ReadFromJsonAsync<ApiResponse<List<MyAuctionsDTO>>> ();

            if (responseData == null || responseData.body == null) {
                return;
            }

            if (responseData?.body != null)
                Auctions = new List<MyAuctionsDTO> ();
            foreach (MyAuctionsDTO auc in responseData.body) {
                MyAuctionsDTO aux = new MyAuctionsDTO {
                    Id = auc.Id,
                    Name = auc.Name,
                    FirstPrice = auc.FirstPrice,
                    DateStart = auc.DateStart,
                    DateEnd = auc.DateEnd,
                    BidsCount = auc.BidsCount,
                    LastPrice = auc.LastPrice,
                    Status = auc.Status,
                    ImageBase64 = auc.ImageBase64,
                    MimeImage = auc.MimeImage
                };

                Auctions.Add (aux);
            }
        }
    }
}
