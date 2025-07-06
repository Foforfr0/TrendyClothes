using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Json;
using AuctionParticipantService.Models;
using WebPage.DTO;
using System.Text.Json;

namespace WebPage.Pages.Auctions
{
    public class ConsultAuctionsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ConsultAuctionsModel> _logger;

        public List<AuctionDTO> Auctions { get; set; } = new();

        public ConsultAuctionsModel(IHttpClientFactory httpClientFactory, ILogger<ConsultAuctionsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var updateResponse = await client.PutAsync("http://apigateway/api/Auctions/Auction/UpdateExpiredAuctions", null);

                string updateJson = await updateResponse.Content.ReadAsStringAsync();
                Console.WriteLine("PUT UpdateExpiredAuctions JSON: " + updateJson);

                if (!updateResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("No se pudo actualizar subastas vencidas. Status: " + updateResponse.StatusCode);
                }

                var response = await client.GetAsync("http://apigateway/api/Auctions/Auction");
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("GET Auctions JSON: " + json);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse<List<AuctionDTO>>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.body != null)
                        Auctions = result.body;
                }
                else
                {
                    Console.WriteLine("Status Code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consumir la API: " + ex.Message);
            }
        }

    }
}
