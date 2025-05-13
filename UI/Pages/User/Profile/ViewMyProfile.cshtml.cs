using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.DTO.User.Profile;

namespace UI.Pages.User.Profile {
    public class ViewMyProfileModel : PageModel {
        [BindProperty]
        public MyPersonalInformationDTO _currentUser {
            get; set;
        }
        [BindProperty]
        public AddressDTO _addressDTO {
            get; set;
        }

        public void OnGet () {
        }
    }
}
