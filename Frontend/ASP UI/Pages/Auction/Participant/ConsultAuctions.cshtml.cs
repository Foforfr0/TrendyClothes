using AuctionParticipantService.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebPage.Connections;
using WebPage.DTO;
using Microsoft.Extensions.Options;

namespace WebPage.Pages.Auctions {
    public class ConsultAuctionsModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;
        private readonly ILogger<ConsultAuctionsModel> _logger;

        public List<AuctionDTO> Auctions { get; set; } = new ();

        public ConsultAuctionsModel (IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> services, ILogger<ConsultAuctionsModel> logger) {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
            _logger = logger;
        }

        public async Task OnGetAsync () {
            try {
                var client = _httpClientFactory.CreateClient ();

                var updateResponse = await client.PutAsync ($"http://apigateway{_services.REST.Auction.Auction.UpdateExpiredAuctions}", null);

                string updateJson = await updateResponse.Content.ReadAsStringAsync ();
                Console.WriteLine ("PUT UpdateExpiredAuctions JSON: " + updateJson);

                if (!updateResponse.IsSuccessStatusCode) {
                    Console.WriteLine ("No se pudo actualizar subastas vencidas. Status: " + updateResponse.StatusCode);
                }

                var response = await client.GetAsync ($"http://apigateway{_services.REST.Auction.Auction.GetAuctions}");
                string json = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"GET Auctions JSON from http://apigateway{_services.REST.Auction.Auction.GetAuctions}: " + json);

                if (response.IsSuccessStatusCode) {
                    var result = JsonSerializer.Deserialize<ApiResponse<List<AuctionDTO>>> (json, new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.body != null)
                        Auctions = result.body;
                } else {
                    Console.WriteLine ("Status Code: " + response.StatusCode);
                }
            } catch (Exception ex) {
                Console.WriteLine ("Error al consumir la API: " + ex.Message);
            }
        }

    }
}
