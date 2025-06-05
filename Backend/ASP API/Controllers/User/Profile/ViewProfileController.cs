using Backend.DTO;
using Backend.DTO.User.Profile;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Profile {
    [ApiController]
    [Authorize]
    [Route ("api/User/[controller]")]
    public class ViewProfileController : Controller {
        private readonly IConsultProfileService _profileService;

        public ViewProfileController (IConsultProfileService profileService) {
            _profileService = profileService;
        }

        [Authorize]
        [HttpGet ("GetPersonalData")]
        public async Task<IActionResult> GetMyData ([FromQuery] string? username) {
            try {
                if (string.IsNullOrEmpty (username))
                    username = User.Identity?.Name;
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario no encontrado.");
                MessageResponse<MyPersonalInformationDTO> response = await _profileService.GetMyDataInformation (username);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved == null)
                    return NotFound (new {
                        response.Message
                    });
                return Ok (new {
                    response.Message,
                    body = response.DataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.ToString ());
            }
        }

        [Authorize]
        [HttpGet ("GetAddresses")]
        public async Task<IActionResult> GetMyAddresses ([FromQuery] string? username) {
            try {
                if (string.IsNullOrEmpty (username))
                    username = User.Identity?.Name;
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Nombre de usuario no encontrado." + User.Identity?.Name ?? "Usuario nulo.");
                MessageResponse<List<AddressDTO>> response = await _profileService.GetAddressesAsync (username);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved == null || response.DataRetrieved.Count <= 0)
                    return NotFound (response.Message);
                return Ok (new {
                    response.Message,
                    body = response.DataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}