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
        private readonly ServicesBuilder _servicesBuilder;

        public PaymentModel(
            IHttpClientFactory httpClientFactory,
            IOptions<ServicesConfig> services,
            ILogger<PaymentModel> logger,
            ServicesBuilder servicesBuilder)
        {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
            _logger = logger;
            _servicesBuilder = servicesBuilder;
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
                var client = _httpClientFactory.CreateClient();

                string cookies = HttpContext.Request.Headers["Cookie"].ToString();
                if (!string.IsNullOrEmpty(cookies))
                    client.DefaultRequestHeaders.Add("Cookie", cookies);

                string patchUrl = _servicesBuilder.AuctioneerPatchAuctionUrl;
                _logger.LogInformation("URL de PATCH usada en pago: {url}", patchUrl);

                var patchBody = new
                {
                    auctionId = this.AuctionId,
                    statusId = SubastaPagadaStatusId
                };

                var jsonContent = JsonContent.Create(
                    patchBody,
                    options: new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                    }
                );
                _logger.LogWarning(" PATCH BODY: {json}", System.Text.Json.JsonSerializer.Serialize(patchBody));

                var patchResponse = await client.PatchAsync(patchUrl, jsonContent);

                if (!patchResponse.IsSuccessStatusCode)
                {
                    TempData["StatusChangeError"] = "No se pudo actualizar el estado.";
                    return RedirectToPage("/Auction/Participant/ConsultAuctions");
                }

                var result = await patchResponse.Content.ReadFromJsonAsync<ApiResponse<bool>>();
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
