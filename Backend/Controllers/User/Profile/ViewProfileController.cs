using Backend.Auth;
using Backend.DTO;
using Backend.DTO.User.Profile;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Profile {
    [Authorize]
    [ApiController]
    [Route ("api/User/[controller]")]
    public class ViewProfileController : Controller {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ManageJWTToken _manageJWTToken;
        private readonly IProfileService _profileService;

        public ViewProfileController (IHttpContextAccessor contextAccessor, ManageJWTToken manageJWTToken, IProfileService profileService) {
            _contextAccessor = contextAccessor;
            _manageJWTToken = manageJWTToken;
            _profileService = profileService;
        }

        [HttpGet ("GetPersonalData")]
        public async Task<IActionResult> GetMyData ([FromQuery] string? username) {
            try {
                if (string.IsNullOrEmpty (username)) {
                    username = _manageJWTToken.GetUsernameFromCookie ();
                }
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario no encontrado.");
                MessageResponse<MyPersonalInformationDTO> response = await _profileService.GetMyDataInformation (username);
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
                return HttpResponses.InternalServerError (ex.ToString ());
            }
        }

        [HttpGet ("GetAddresses")]
        public async Task<IActionResult> GetMyAddresses ([FromQuery] string? username) {
            try {
                if (string.IsNullOrEmpty (username)) {
                    username = _manageJWTToken.GetUsernameFromCookie ();
                }
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario no encontrado.");
                MessageResponse<List<AddressDTO>> response = await _profileService.GetAddressesAsync (username);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null || response.dataRetrieved.Count <= 0)
                    return NotFound (response.message);
                return Ok (new {
                    response.message,
                    body = new JsonResult (response.dataRetrieved)
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}