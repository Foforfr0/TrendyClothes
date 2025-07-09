using AuctionParticipantService.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using WebPage.Connections;
using WebPage.DTO;

namespace WebPage.Pages.Auctions
{
    public class ConsultAuctionsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;
        private readonly ILogger<ConsultAuctionsModel> _logger;

        public List<AuctionDTO> Auctions { get; set; } = new();
        public List<AuctionDTO> WonAuctions { get; set; } = new();

        public ConsultAuctionsModel(IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> services, ILogger<ConsultAuctionsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                await client.PutAsync($"http://apigateway{_services.REST.Auction.Auction.UpdateExpiredAuctions}", null);

                var activeResponse = await client.GetAsync($"http://apigateway{_services.REST.Auction.Auction.GetAuctions}");
                if (activeResponse.IsSuccessStatusCode)
                {
                    var json = await activeResponse.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse<List<AuctionDTO>>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.body != null)
                        Auctions = result.body;
                }

                string? username = HttpContext.User.Identity?.Name;
                if (!string.IsNullOrWhiteSpace(username))
                {
                    var requestBody = new StringContent(JsonSerializer.Serialize(username), Encoding.UTF8, "application/json");

                    var wonResponse = await client.GetAsync($"http://apigateway{_services.REST.Auction.Auction.GetWonAuctions}?username={Uri.EscapeDataString(username)}");

                    if (wonResponse.IsSuccessStatusCode)
                    {
                        var wonJson = await wonResponse.Content.ReadAsStringAsync();
                        var wonResult = JsonSerializer.Deserialize<ApiResponse<List<AuctionDTO>>>(wonJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (wonResult?.body != null)
                            WonAuctions = wonResult.body;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar subastas.");
            }
        }
    }
}
