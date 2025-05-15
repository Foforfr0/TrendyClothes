using Backend.Auth;
using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Auth {
    [ApiController]
    [Route ("api/User/Auth/[controller]")]
    public class LoginController : ControllerBase {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        private readonly ManageJWTToken _manageJWTToken;

        public LoginController (IConfiguration iConfig, IAuthService authService, ManageJWTToken manageJWTToken) {
            _config = iConfig;
            _authService = authService;
            _manageJWTToken = manageJWTToken;
        }

        [HttpPost ("Login")] // POST
        public async Task<IActionResult> PostLoginAsync ([FromBody] LoginDTO loginDTO) {
            try {
                if (loginDTO == null || loginDTO.username.Length <= 0 || loginDTO.password.Length <= 0)
                    return BadRequest ("Campos vacíos.");
                MessageResponse<LoginDTO> response = await _authService.PostLoginAsync (loginDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null)
                    return Unauthorized (new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                    body = new JsonResult (response.dataRetrieved)
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("ValidateEmailUser")] // GET
        public async Task<IActionResult> GetValidateEmailUserAsync ([FromQuery] EmailDTO emailDTO) {
            try {
                if (emailDTO == null || emailDTO.username.Length <= 0 || emailDTO.email.Length <= 0)
                    return BadRequest ("Campos vacíos.");
                MessageResponse<EmailDTO> response = await _authService.GetValidateEmailUserAsync (emailDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null)
                    return NotFound (new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                    body = new JsonResult (response.dataRetrieved)
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpPost ("CreateTwoFactorCode")] // POST
        public async Task<IActionResult> PostTwoFactorCodeAsync ([FromBody] EmailDTO emailDTO) {
            try {
                if (emailDTO == null || string.IsNullOrEmpty (emailDTO.username) || string.IsNullOrEmpty (emailDTO.email))
                    return BadRequest ("Campos vacíos.");
                MessageResponse<bool> response = await _authService.PostTwoFactorCodeAsync (emailDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == false)
                    return NotFound (new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("ValidateTwoFactorCode")] // GET
        public async Task<IActionResult> GetValidateTwoFactorCode ([FromQuery] CodeTwoFactorDTO codeTwoFactorDTO) {
            try {
                if (codeTwoFactorDTO == null || codeTwoFactorDTO.username.Length <= 0 || codeTwoFactorDTO.twoFactorCode.Length <= 0)
                    return BadRequest ("Campos vacíos.");
                MessageResponse<jwtDTO> response = await _authService.GetValidateTwoFactorCode (codeTwoFactorDTO);
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    jwtToken = _manageJWTToken.GenerateToken (response.dataRetrieved.username, response.dataRetrieved.role);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    Response.Cookies.Append ("jwtToken", jwtToken, new CookieOptions {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes (int.Parse (_config["Jwt:ExpiresInMinutes"] ?? "30"))
                    });
                } catch (Exception ex) {
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

        [HttpDelete ("DeleteTwoFactorCode")] // DELETE
        public async Task<IActionResult> DeleteTwoFactorCodeAsync ([FromQuery] string username) {
            try {
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Campo vacío.");
                MessageResponse<bool> response = await _authService.DeleteTwoFactorCodeAsync (username);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == false)
                    return NotFound (new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpPost ("Logout")]
        public IActionResult PostLogout () {
            try {
                Response.Cookies.Delete ("jwtToken");
                return Ok ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
