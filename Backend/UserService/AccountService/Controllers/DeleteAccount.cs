using AccountService.Models;
using AccountService.Services.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/User/[controller]")]
    public class DeleteAccount : Controller {
        private readonly IDeleteAccountService _deleteAccountService;

        public DeleteAccount (IDeleteAccountService deleteAccountService) {
            _deleteAccountService = deleteAccountService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser ([FromQuery] string username) {
            try {
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario requerido.");
                MessageResponse<bool> response = await _deleteAccountService.DeleteUserAsync (username);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (!response.DataRetrieved)
                    return Conflict (new {
                        response.Message
                    });
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
