using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI_ASP.DTO.User.Profile;

namespace UI_ASP.Pages.User.Profile {
    public class ViewMyProfileModel : PageModel {
        [BindProperty]
        public MyPersonalInformationDTO? currentUser { get; set; } = new MyPersonalInformationDTO ();

        [BindProperty]
        public List<AddressDTO>? addresses { get; set; } = new List<AddressDTO> ();

        public void OnGet () {

        }
    }
}