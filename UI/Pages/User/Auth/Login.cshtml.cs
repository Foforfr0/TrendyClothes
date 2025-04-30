using Backend.DTO.User.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.User.Auth {
    public class LoginModel : PageModel {
        [BindProperty]
        public LoginDTO _loginDTO {
            get; set;
        }

        public void OnGet () {
        }
    }
}
