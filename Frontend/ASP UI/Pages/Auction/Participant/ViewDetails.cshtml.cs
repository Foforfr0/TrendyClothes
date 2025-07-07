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

        [BindProperty]
        public decimal ShownLastPrice { get; set; }

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
                    {
                        Auction = result.body;
                        ShownLastPrice = Auction.LastPrice ?? 0;
                    }
                    else
                    {
                        return NotFound();
                    }
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

                var getResponse = await client.GetAsync($"http://apigateway/api/Auctions/Auction/ById/{Id}");
                string getJson = await getResponse.Content.ReadAsStringAsync();
                Console.WriteLine("GET JSON (POST): " + getJson);

                if (!getResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al verificar el precio actual.";
                    return RedirectToPage("/Auction/Participant/ViewDetails", new { id = Id });
                }

                var currentAuction = JsonSerializer.Deserialize<ApiResponse<AuctionDTO>>(getJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })?.body;

                if (currentAuction == null)
                {
                    TempData["ErrorMessage"] = "No se encontró la subasta actual.";
                    return RedirectToPage("/Auction/Participant/ViewDetails", new { id = Id });
                }

                if ((currentAuction.LastPrice ?? 0m) != ShownLastPrice)
                {
                    TempData["ErrorMessage"] = "La puja ha cambiado desde la última vez. Por favor actualiza para ver el nuevo precio.";
                    return RedirectToPage("/Auction/Participant/ViewDetails", new { id = Id });
                }

                string? username = HttpContext.User.Identity?.Name;

                if (string.IsNullOrEmpty(username))
                {
                    TempData["ErrorMessage"] = "Sesión no válida. Inicia sesión para pujar.";
                    return RedirectToPage("/User/Auth/Login");
                }

                var increaseResponse = await client.PutAsync(
                    $"http://apigateway/api/Auctions/Auction/IncreaseBid/{Id}", null
                );

                if (!increaseResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al incrementar el precio.";
                    return RedirectToPage("/Auction/Participant/ViewDetails", new { id = Id });
                }

               

                var bidPayload = new { AuctionId = Id, Username = username };
                var bidContent = new StringContent(JsonSerializer.Serialize(bidPayload), Encoding.UTF8, "application/json");

                var registerResponse = await client.PostAsync("http://apigateway/api/Auctions/Auction/RegisterBid", bidContent);

                if (!registerResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al registrar la puja.";
                    return RedirectToPage("/Auction/Participant/ViewDetails", new { id = Id });
                }

                TempData["SuccessMessage"] = "¡Puja registrada exitosamente!";
                return RedirectToPage("/Auction/Participant/ConsultAuctions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la operación de puja");
                TempData["ErrorMessage"] = "Ha ocurrido un error inesperado.";
                return RedirectToPage("/Auction/Participant/ViewDetails", new { id = Id });
            }
        }
    }
}
