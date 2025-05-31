using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO.User.Registration;

namespace WebPage.Pages.User.Auth {
    public class CreateAccountModel : PageModel {
        [BindProperty]
        public required RegistrationUserDTO newUserDTO {
            get; set;
        }
        public void OnGet () {
        }
    }
}
