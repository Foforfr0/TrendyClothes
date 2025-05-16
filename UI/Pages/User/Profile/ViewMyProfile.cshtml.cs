using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.DTO;
using UI.DTO.User.Profile;

namespace UI.Pages.User.Profile {
    public class ViewMyProfileModel : PageModel {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ViewMyProfileModel (IHttpClientFactory httpClientFactory, IConfiguration config) {
            _httpClient = httpClientFactory.CreateClient ();
            _config = config;

        }
        [BindProperty]
        public MyPersonalInformationDTO currentUser { get; set; } = new MyPersonalInformationDTO ();

        [BindProperty]
        public List<AddressDTO> addresses { get; set; } = new List<AddressDTO> ();

        public async Task OnGetAsync () {
            try {
                ApiResponse<MyPersonalInformationDTO>? userResult = 
                    await _httpClient.GetFromJsonAsync<ApiResponse<MyPersonalInformationDTO>> (
                    "https://localhost:5001/api/User/Profile/ViewProfile/GetPersonalData");
                if (userResult != null && userResult.body != null)
                    currentUser = userResult.body.value;

                ApiResponse<List<AddressDTO>>? addressResult = 
                    await _httpClient.GetFromJsonAsync<ApiResponse<List<AddressDTO>>> (
                    "https://localhost:5001/api/User/Profile/ViewProfile/GetAddresses");
                if (addressResult != null && addressResult.body != null)
                    addresses = addressResult.body.value;
            } catch (Exception ex) {
                Console.WriteLine ("Error al obtener perfil: " + ex.ToString ());
            }
        }
    }
}