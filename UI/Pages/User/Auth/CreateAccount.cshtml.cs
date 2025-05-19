using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.DTO.User.Registration;

namespace UI.Pages.User.Auth {
    public class CreateAccountModel : PageModel {
        [BindProperty]
        public required RegistrationUserDTO newUserDTO {
            get; set;
        }
        public void OnGet () {
        }
    }
}
