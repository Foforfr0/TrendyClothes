using AccountService.Models;
using AccountService.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers {
    [ApiController]
    [Route ("api/User/[controller]")]
    public class RegistrationController : Controller {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRegistrationAccountService _registrationService;

        public RegistrationController (IHttpContextAccessor httpContextAccessor, IRegistrationAccountService registrationService) {
            _contextAccessor = httpContextAccessor;
            _registrationService = registrationService;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser ([FromBody] RegistrationUserDTO newUserDTO) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest (new {
                        message = $"Los datos del nuevo usuario son inválidos."
                    });
                MessageResponse<bool> response = await _registrationService.PostUserAsync (newUserDTO);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (!response.DataRetrieved)
                    return Conflict (new {
                        response.Message
                    });
                return Ok (new {
                    response.Message
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
