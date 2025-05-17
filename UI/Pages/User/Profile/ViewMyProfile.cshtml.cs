using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.DTO.User.Profile;

namespace UI.Pages.User.Profile {
    public class ViewMyProfileModel : PageModel {
        [BindProperty]
        public MyPersonalInformationDTO? currentUser { get; set; } = new MyPersonalInformationDTO ();

        [BindProperty]
        public List<AddressDTO>? addresses { get; set; } = new List<AddressDTO> ();

        public async Task OnGetAsync () {

        }
    }
}