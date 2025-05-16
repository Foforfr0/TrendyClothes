using Backend.Auth;
using Backend.DTO;
using Backend.DTO.User.Profile;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Profile {
    [ApiController]
    [Route ("api/User/Profile/[controller]")]
    public class ViewProfileController : Controller {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IProfileService _profileService;
        private readonly ManageJWTToken _manageJWTToken;

        public ViewProfileController (IHttpContextAccessor contextAccessor, IProfileService profileService, ManageJWTToken manageJWTToken) {
            _contextAccessor = contextAccessor;
            _profileService = profileService;
            _manageJWTToken = manageJWTToken;
        }

        [HttpGet ("GetPersonalData")]
        public async Task<IActionResult> GetMyDataAsync ([FromQuery] string? username) {
            try {
                if (string.IsNullOrEmpty (username)) {
                    username = _manageJWTToken.GetUsernameFromCookie ();
                }
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario no encontrado.");
                MessageResponse<MyPersonalInformationDTO> response = await _profileService.GetMyPersonalInformation (username);
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
        public async Task<IActionResult> GetMyAddressAsync ([FromQuery] string? username) {
            try {
                if (string.IsNullOrEmpty (username)) {
                    username = _manageJWTToken.GetUsernameFromCookie ();
                }
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario no encontrado.");
                MessageResponse<List<AddressDTO>> response = await _profileService.GetAddressAsync (username);
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