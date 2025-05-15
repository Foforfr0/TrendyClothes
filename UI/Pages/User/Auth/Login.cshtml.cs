using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.DTO.User.Auth;

namespace UI.Pages.User.Auth {
    public class LoginModel : PageModel {
        [BindProperty]
        public required LoginDTO loginDTO {
            get; set;
        }
        [BindProperty]
        public required EmailDTO emailDTO {
            get; set;
        }
        [BindProperty]
        public CodeTwoFactorDTO? codeTwoFactorDTO {
            get; set;
        }

        public void OnGet () {
        }
    }
}
