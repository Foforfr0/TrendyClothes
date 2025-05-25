using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;
using WebPage.DTO.User.Auth;

namespace WebPage.Pages.User.Auth {
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
