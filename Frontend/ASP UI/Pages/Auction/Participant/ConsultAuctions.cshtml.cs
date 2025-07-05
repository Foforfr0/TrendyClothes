using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebPage.Connections;
using AuctionParticipantService.Models;
using WebPage.DTO;

namespace WebPage.Pages.Auctions
{
    public class ConsultAuctionsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ConsultAuctionsModel> _logger;
        private readonly ServicesBuilder _services;

        public List<AuctionDTO> Auctions { get; set; } = new();

        public ConsultAuctionsModel(IHttpClientFactory httpClientFactory, ServicesBuilder services, ILogger<ConsultAuctionsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            string requestURL = "http://apigateway/api/Auctions/Auction";
            _logger.LogInformation("ConsultAuctionsModel.OnGetAsync: " + requestURL);

            HttpResponseMessage response = await httpClient.GetAsync(requestURL);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = await response.Content.ReadFromJsonAsync<List<AuctionDTO>>();
                if (data != null && data.Count > 0)
                    Auctions = data;
            }
        }
    }
}