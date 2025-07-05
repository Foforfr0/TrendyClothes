using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using AuctionParticipantService.Models;
using WebPage.DTO;
using System.Text;
using System.Text.Json;

namespace WebPage.Pages.Auctions
{
    public class ViewDetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ViewDetailsModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public AuctionDTO? Auction { get; set; }

        public ViewDetailsModel(IHttpClientFactory httpClientFactory, ILogger<ViewDetailsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://apigateway/api/Auctions/Auction/ById/{Id}");

                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("GET JSON: " + json);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse<AuctionDTO>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.body != null)
                        Auction = result.body;
                    else
                        return NotFound();
                }
                else
                {
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles de la subasta");
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // 1. Incrementar el precio
                var increaseResponse = await client.PutAsync(
                    $"http://apigateway/api/Auctions/Auction/IncreaseBid/{Id}",
                    null
                );

                var increaseJson = await increaseResponse.Content.ReadAsStringAsync();
                Console.WriteLine("PUT JSON: " + increaseJson);

                if (!increaseResponse.IsSuccessStatusCode)
                    return StatusCode(500, "Error al incrementar el precio.");

                // 2. Registrar la puja
                var bidPayload = new
                {
                    AuctionId = Id,
                    BuyerId = 1 // fijo para pruebas
                };

                var bidContent = new StringContent(
                    JsonSerializer.Serialize(bidPayload),
                    Encoding.UTF8,
                    "application/json"
                );

                var registerResponse = await client.PostAsync("http://apigateway/api/Auctions/Auction/RegisterBid", bidContent);
                var registerJson = await registerResponse.Content.ReadAsStringAsync();
                Console.WriteLine("POST JSON: " + registerJson);

                if (!registerResponse.IsSuccessStatusCode)
                    return StatusCode(500, "Error al registrar la puja.");

                TempData["SuccessMessage"] = "Puja registrada con exito...";
                return RedirectToPage("/Auction/Participant/ConsultAuctions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la operación de puja");
                return StatusCode(500);
            }
        }
    }
}
