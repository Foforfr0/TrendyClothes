using Backend.DTO;
using Backend.DTO.User.Registration;
using Backend.Services.Intefaces.User;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User {
    [ApiController]
    [Route ("api/User/[controller]")]
    public class RegistrationController : Controller {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRegistrationService _registrationService;

        public RegistrationController (IHttpContextAccessor httpContextAccessor, IRegistrationService registrationService) {
            _contextAccessor = httpContextAccessor;
            _registrationService = registrationService;
        }

        [HttpPost ("AddUser")]
        public async Task<IActionResult> PostUser ([FromBody] RegistrationUserDTO newUserDTO) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest (new {
                        message = $"Los datos del nuevo usuario son inválidos.",
                        error = ModelState.GetErrors ()
                    });
                MessageResponse<bool> response = await _registrationService.PostUserAsync (newUserDTO);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (!response.dataRetrieved)
                    return Conflict (new {
                        response.message
                    });
                return Ok (new {
                    response.message
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpDelete ("DeleteUser")]
        public async Task<IActionResult> DeleteUser ([FromQuery] string username) {
            try {
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario requerido.");
                MessageResponse<bool> response = await _registrationService.DeleteUserAsync (username);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (!response.dataRetrieved)
                    return Conflict (new {
                        response.message
                    });
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
