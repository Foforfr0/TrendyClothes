using Backend.DTO.User.Auth;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Auth {
    [ApiController]
    [Route ("api/User/Auth/[controller]")]
    public class LoginController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginController> _logger;

        public LoginController (IAuthService authService,
                                ILogger<LoginController> logger) {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost ("Login")]
        public async Task<IActionResult> PostLoginAsync ([FromBody] LoginDTO loginDTO) {
            try {
                if (loginDTO == null || loginDTO.Username.Length <= 0 || loginDTO.Password.Length <= 0) {
                    return BadRequest ("Campos vacíos.");
                } else {
                    LoginDTO? response = await _authService.PostLoginAsync (loginDTO);
                    if (response == null) {
                        return Unauthorized (new {
                            message = "Usuario o contraseña incorrectos."
                        });
                    } else {
                        return Ok (new {
                            message = "Usuario encontrado.",
                            body = new {
                                Username = response.Username,
                                Password = response.Password
                            }
                        });
                    }
                }
            } catch (Exception ex) {
                return StatusCode (StatusCodes.Status500InternalServerError, new {
                    message = "Error interno del servidor.",
                    error = ex.Message // Considera no devolver el mensaje de error en producción
                });
            }
        }
    }
}
