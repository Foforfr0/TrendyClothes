using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI_ASP.DTO.User.Registration;

namespace UI_ASP.Pages.User.Auth {
    public class CreateAccountModel : PageModel {
        [BindProperty]
        public required RegistrationUserDTO newUserDTO {
            get; set;
        }
        public void OnGet () {
        }
    }
}
