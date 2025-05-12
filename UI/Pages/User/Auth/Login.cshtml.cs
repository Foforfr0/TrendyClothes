using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.DTO.User.Auth;

namespace UI.Pages.User.Auth {
    public class LoginModel : PageModel {
        [BindProperty]
        public required LoginDTO _loginDTO {
            get; set;
        }
        [BindProperty]
        public required EmailDTO _emailDTO {
            get; set;
        }
        [BindProperty]
        public CodeTwoFactorDTO _CodeTwoFactorDTO {
            get; set;
        }

        public void OnGet () {
        }
    }
}
