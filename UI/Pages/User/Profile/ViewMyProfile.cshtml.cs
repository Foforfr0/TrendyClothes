using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.DTO.User.Profile;

namespace UI.Pages.User.Profile {
    public class ViewMyProfileModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ViewMyProfileModel (IHttpClientFactory httpClientFactory, IConfiguration config) {
            _httpClientFactory = httpClientFactory;
            _config = config;

        }
        [BindProperty]
        public MyPersonalInformationDTO currentUser { get; set; } = new MyPersonalInformationDTO ();

        [BindProperty]
        public List<AddressDTO> addresses { get; set; } = new List<AddressDTO> ();

        public async Task OnGetAsync () {
            try {
                HttpClient? client = _httpClientFactory.CreateClient ();
                if (Request.Cookies.TryGetValue ("jwt", out string jwt))
                    client.DefaultRequestHeaders.Add ("Cookie", $"jwt={jwt}");
                string? baseUrl = _config["BackendSettings:BACKEND_URL"] ?? "https://localhost:5001";

                MyPersonalInformationDTO? userResult = await client.GetFromJsonAsync<MyPersonalInformationDTO> (
                    $"{baseUrl}/api/User/Profile/GetMyData");

                if (userResult != null)
                    currentUser = userResult;

                List<AddressDTO>? addressResult = await client.GetFromJsonAsync<List<AddressDTO>> (
                    $"{baseUrl}/api/User/Profile/GetAddresses");

                if (addressResult != null)
                    addresses = addressResult;
            } catch (Exception ex) {
                Console.WriteLine ("Error al obtener perfil: " + ex.ToString());
            }
        }
    }
}