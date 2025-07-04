using AuctionParticipantService.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using WebPage.DTO.Auction;

namespace WebPage.Pages.Auction
{
    public class ConsultAuctionsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConsultAuctionsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<AuctionDTO>? Auctions { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync("/api/auction/active");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Auctions = JsonSerializer.Deserialize<List<AuctionDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
            {
                Auctions = new List<AuctionDTO>();
            }
        }
    }
}
