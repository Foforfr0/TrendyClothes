using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Auction;

namespace WebPage.Pages.Auction.Participant
{
    public class ConsultAuctionsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ConsultAuctionsModel> _logger;
        private readonly ServicesBuilder _services;

        public List<AuctionsListDTO> Auctions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? query { get; set; }

        public ConsultAuctionsModel(
            IHttpClientFactory httpClientFactory,
            ServicesBuilder services,
            ILogger<ConsultAuctionsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                string requestUrl = $"http://apigateway/api/Auction/Participant/Auctions";
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<AuctionsListDTO>>>();
                    if (result?.body != null)
                        Auctions = result.body;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener subastas: {ex.Message}");
            }
        }
    }
}
