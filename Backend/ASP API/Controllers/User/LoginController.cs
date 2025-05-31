using Backend.Auth;
using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Controllers.User {
    [ApiController]
    [Route ("api/User/[controller]")]
    public class LoginController : ControllerBase {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<LoginController> _logger;
        private readonly ManageJWTToken _manageJWTToken;
        private readonly IAuthService _authService;

        public LoginController (IHttpContextAccessor contextAccesor, ILogger<LoginController> logger, ManageJWTToken manageJWTToken, IAuthService authService) {
            _contextAccessor = contextAccesor;
            _logger = logger;
            _manageJWTToken = manageJWTToken;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> PostLogin ([FromBody] LoginDTO loginDTO) {
            try {
                if (!ModelState.IsValid || loginDTO == null || loginDTO.username.Length <= 0 || loginDTO.password.Length <= 0)
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
                MessageResponse<LoginDTO> response = await _authService.ValidateLoginAsync (loginDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null)
                    return Unauthorized (new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                    body = response.dataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("ValidateEmailUser")]
        public async Task<IActionResult> GetValidateEmailUser ([FromQuery] EmailDTO emailDTO) {
            try {
                if (!ModelState.IsValid || emailDTO == null || emailDTO.username.Length <= 0 || emailDTO.email.Length <= 0)
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
                MessageResponse<EmailDTO> response = await _authService.ValidateEmailUserAsync (emailDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null)
                    return NotFound (new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                    body = response.dataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpPost ("CreateTwoFactorCode")]
        public async Task<IActionResult> PostTwoFactorCode ([FromBody] EmailDTO emailDTO) {
            try {
                if (!ModelState.IsValid || emailDTO == null || string.IsNullOrEmpty (emailDTO.username) || string.IsNullOrEmpty (emailDTO.email))
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
                MessageResponse<bool> response = await _authService.PostTwoFactorCodeAsync (emailDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == false)
                    return NotFound (new {
                        response.message
                    });
                return Ok (new {
                    response.message
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpPost ("ValidateTwoFactorCode")]
        public async Task<IActionResult> GetValidateTwoFactorCode ([FromBody] CodeTwoFactorDTO codeTwoFactorDTO) {
            try {
                if (!ModelState.IsValid || codeTwoFactorDTO == null || codeTwoFactorDTO.username.Length <= 0 || codeTwoFactorDTO.twoFactorCode.Length <= 0)
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
                MessageResponse<jwtDTO> response = await _authService.ValidateTwoFactorCode (codeTwoFactorDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null && response.message.Equals ("Usuario no encontrado."))
                    return NotFound (new {
                        response.message
                    });
                if (response.dataRetrieved == null && response.message.Equals ("Usuario no posee un código doble factor."))
                    return NotFound (new {
                        response.message
                    });
                if (response.dataRetrieved == null && response.message.Equals ("Código doble factor incorrecto."))
                    return Unauthorized (new {
                        response.message
                    });

                string jwtToken = "";
                try {
                    jwtToken = _manageJWTToken.GenerateToken (response.dataRetrieved?.username ?? "No logged in.", response.dataRetrieved.role);
                    _logger.LogInformation ("JWT TOKEN " + jwtToken);
                    _contextAccessor.HttpContext?.Response.Cookies.Append ("jwtToken", jwtToken, new CookieOptions {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays (7),
                        IsEssential = true,
                        Path = "/",
                    });

                    JwtSecurityTokenHandler? handler = new JwtSecurityTokenHandler ();
                    JwtSecurityToken? jwtSecurityToken = handler.ReadJwtToken (jwtToken);

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

                    _logger.LogInformation ("jwtToken cookie created and appended to Response.Cookies");
                    _logger.LogInformation ("After HttpContext.SignInAsync");
                } catch (Exception ex) {
                    _logger.LogCritical ("Exepcion: " + ex.ToString ());
                    return HttpResponses.InternalServerError (ex.ToString ());
                }

                return Ok (new {
                    response.message,
                    jwtToken
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpDelete ("DeleteTwoFactorCode")]
        public async Task<IActionResult> DeleteTwoFactorCode ([FromQuery] string username) {
            try {
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario inválido.");
                MessageResponse<bool> response = await _authService.DeleteTwoFactorCodeAsync (username);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == false)
                    return NotFound (new {
                        response.message
                    });
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
