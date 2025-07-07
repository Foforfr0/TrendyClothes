using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using WebPage.Connections;
using WebPage.DTO.User.Auth;
using Microsoft.Extensions.Options;

namespace WebPage.Pages.User.Auth {
    public class LoginModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfig _services;
        private readonly ILogger<LoginModel> _logger;

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

        public LoginModel (IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> services, ILogger<LoginModel> logger) {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
            _logger = logger;
        }

        public void OnGet () {
        }

        public async Task<IActionResult> OnPostFinalValidationAsync () {
            try {
                string username = Request.Form["username"];
                string twoFactorCode = Request.Form["twoFactorCode"];
                _logger.LogInformation ("username = " + username + "twoFactorCode = " + twoFactorCode);

                if (string.IsNullOrWhiteSpace (codeTwoFactorDTO.username) || string.IsNullOrWhiteSpace (codeTwoFactorDTO.twoFactorCode)) {
                    ModelState.AddModelError (string.Empty, "Usuario y código son requeridos.");
                    return Page ();
                }
                HttpClient httpClient = _httpClientFactory.CreateClient ();
                string requestURL = $"http://apigateway{_services.REST.User.Auth.Login.ValidateTwoFactorCode}";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync (requestURL, new {
                    username, twoFactorCode
                });

                _logger.LogInformation ("Status code: " + response.StatusCode);

                if (response.StatusCode == HttpStatusCode.NotFound) {
                    ModelState.AddModelError (string.Empty, "Usuario no encontrado o código doble factor no asignado.");
                    return Page ();
                }

                if (response.StatusCode == HttpStatusCode.BadRequest) {
                    ModelState.AddModelError (string.Empty, "Datos inválidos.");
                    return Page ();
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized) {
                    ModelState.AddModelError (string.Empty, "No autorizado.");
                    return Page ();
                }

                string? resultJson = await response.Content.ReadAsStringAsync ();
                ValidationTwoFactorCodeResponseDTO? loginResponse = JsonSerializer.Deserialize<ValidationTwoFactorCodeResponseDTO> (resultJson);
                string jwt = loginResponse?.jwtToken;
                _logger.LogInformation ("JWT: " + jwt);

                JwtSecurityTokenHandler? handler = new JwtSecurityTokenHandler ();
                JwtSecurityToken? jwtSecurityToken = handler.ReadJwtToken (jwt);

                string usernameJWT = jwtSecurityToken.Claims.First (c => c.Type == ClaimTypes.Name).Value;
                _logger.LogInformation ("Username: " + usernameJWT);
                string role = jwtSecurityToken.Claims.First (c => c.Type == ClaimTypes.Role).Value;
                _logger.LogInformation ("Role: " + role);
                string sid = jwtSecurityToken.Claims.First (c => c.Type == ClaimTypes.Sid).Value;
                _logger.LogInformation ("SID: " + sid);

                List<Claim>? claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usernameJWT),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Sid, sid)
                };

                ClaimsIdentity? identity = new ClaimsIdentity (claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal? principal = new ClaimsPrincipal (identity);

                AuthenticationProperties? authProperties = new AuthenticationProperties {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays (7),
                    AllowRefresh = true
                };

                Response.Cookies.Append ("jwtToken", jwt, new CookieOptions {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays (7),
                    IsEssential = true,
                    Path = "/",
                });

                await HttpContext.SignInAsync ("signInScheme", principal, authProperties);
                return RedirectToPage ("/User/Profile/ViewMyProfile");
            } catch (Exception ex) {
                _logger.LogError (ex, "Error en FinalValidation");
                ModelState.AddModelError (string.Empty, "Error al validar el código doble factor.");
                return Page ();
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync () {
            await HttpContext.SignOutAsync ("signInScheme");
            return RedirectToPage ("/User/Auth/Login");
        }
    }
}
