using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using WebPage.Connections;
using WebPage.DTO;

namespace WebPage.Pages.Auctions
{
    public class PaymentModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;
        private readonly ILogger<PaymentModel> _logger;

        public PaymentModel(IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> services, ILogger<PaymentModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int AuctionId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Número de tarjeta requerido")]
        [CreditCard(ErrorMessage = "Número de tarjeta no válido")]
        public string CardNumber { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Nombre del titular requerido")]
        public string CardHolder { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Fecha de expiración requerida")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Formato debe ser MM/YY")]
        public string Expiration { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "CVV requerido")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV de 3 o 4 dígitos")]
        public string CVV { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            const int SubastaPagadaStatusId = 5;

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();
                string cookies = HttpContext.Request.Headers["Cookie"].ToString();

                if (!string.IsNullOrEmpty(cookies))
                {
                    if (httpClient.DefaultRequestHeaders.Contains("Cookie"))
                        httpClient.DefaultRequestHeaders.Remove("Cookie");

                    httpClient.DefaultRequestHeaders.Add("Cookie", cookies);
                }

                string patchUrl = $"http://apigateway/api/Auction/Auctioneer/UpdateAuction";
                var patchBody = new
                {
                    AuctionId = this.AuctionId,
                    StatusId = SubastaPagadaStatusId
                };

                _logger.LogInformation("PATCH a: {url}", patchUrl);

                HttpResponseMessage httpResponse = await httpClient.PatchAsJsonAsync(patchUrl, patchBody);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    TempData["StatusChangeError"] = "No se pudo actualizar el estado.";
                    return RedirectToPage("/Auction/Participant/ConsultAuctions");
                }

                var result = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                if (result == null || !result.body)
                {
                    TempData["StatusChangeError"] = result?.message ?? "Error desconocido al actualizar estado.";
                    return RedirectToPage("/Auction/Participant/ConsultAuctions");
                }

                TempData["SuccessMessage"] = $"Pago simulado exitoso. Estado de subasta #{AuctionId} actualizado.";
                return RedirectToPage("/Auction/Participant/ConsultAuctions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al simular pago y actualizar estado de subasta.");
                TempData["StatusChangeError"] = $"Error inesperado: {ex.Message}";
                return RedirectToPage("/Auction/Participant/ConsultAuctions");
            }
        }
    }
}
