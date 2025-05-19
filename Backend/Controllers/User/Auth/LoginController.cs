using Backend.Auth;
using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers.User.Auth {
    [ApiController]
    [Route ("api/User/[controller]")]
    public class LoginController : ControllerBase {
        private readonly IHttpContextAccessor _contextAccesor;
        private readonly ManageJWTToken _manageJWTToken;
        private readonly IAuthService _authService;

        public LoginController (IHttpContextAccessor contextAccesor, ManageJWTToken manageJWTToken, IAuthService authService) {
            _contextAccesor = contextAccesor;
            _manageJWTToken = manageJWTToken;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> PostLogin ([FromBody] LoginDTO loginDTO) {
            try {
                if (!ModelState.IsValid || loginDTO == null || loginDTO.username.Length <= 0 || loginDTO.password.Length <= 0)
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
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

        [HttpGet ("ValidateEmailUser")]
        public async Task<IActionResult> GetValidateEmailUser ([FromQuery] EmailDTO emailDTO) {
            try {
                if (!ModelState.IsValid || emailDTO == null || emailDTO.username.Length <= 0 || emailDTO.email.Length <= 0)
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
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

        [HttpGet ("ValidateTwoFactorCode")]
        public async Task<IActionResult> GetValidateTwoFactorCode ([FromQuery] CodeTwoFactorDTO codeTwoFactorDTO) {
            try {
                if (!ModelState.IsValid || codeTwoFactorDTO == null || codeTwoFactorDTO.username.Length <= 0 || codeTwoFactorDTO.twoFactorCode.Length <= 0)
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
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
                    Claim[]? claims = new[] {
                        new Claim(ClaimTypes.Name, response.dataRetrieved.username),
                        new Claim(ClaimTypes.Role, response.dataRetrieved.role),
                        new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString())
                    };

                    ClaimsIdentity? identity = new ClaimsIdentity (claims, "Cookies");

                    await HttpContext.SignInAsync ("Cookies", new ClaimsPrincipal (identity));

                    jwtToken = _manageJWTToken.GenerateToken (response.dataRetrieved.username, response.dataRetrieved.role);
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
