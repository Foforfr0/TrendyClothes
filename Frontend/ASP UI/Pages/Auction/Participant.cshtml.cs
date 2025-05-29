using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using WebPage.DTO; // Asegúrate que el namespace sea correcto
using WebPage.DTO.Auction;
using WebPage.DTO.Product.Consult; // Si estás usando ese espacio

namespace WebPage.Pages.Auction
{
    public class ParticipantModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ParticipantModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        [BindProperty]
        public int CustomBid { get; set; }

        public AuctionViewDTO Auction { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<ApiResponse<AuctionViewDTO>>(
                $"{_config["BackendSettings:BackendUrl"]}/api/Auction/{id}");

            if (response?.body is null)
                return NotFound();

            Auction = response.body;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string type)
        {
            var httpClient = _httpClientFactory.CreateClient();

            decimal bidAmount = type == "custom"
                ? CustomBid
                : (Auction.LastPrice ?? 0) + (Auction.MinBid ?? 0);

            var result = await httpClient.PostAsJsonAsync(
                $"{_config["BackendSettings:BackendUrl"]}/api/Auction/bid",
                new { AuctionId = id, UserId = 1, Amount = bidAmount } // Sustituir con ID del usuario real
            );

            if (!result.IsSuccessStatusCode)
            {
                TempData["Error"] = "La puja no fue aceptada.";
                return RedirectToPage(new { id });
            }

            return RedirectToPage(new { id });
        }
    }
}
