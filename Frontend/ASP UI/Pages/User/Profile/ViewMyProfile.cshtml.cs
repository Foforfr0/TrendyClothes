using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO.User.Profile;

namespace WebPage.Pages.User.Profile {
    public class ViewMyProfileModel : PageModel {
        [BindProperty]
        public MyPersonalInformationDTO? currentUser { get; set; } = new MyPersonalInformationDTO ();

        [BindProperty]
        public List<AddressDTO>? addresses { get; set; } = new List<AddressDTO> ();

        public void OnGet () {

        }
    }
}