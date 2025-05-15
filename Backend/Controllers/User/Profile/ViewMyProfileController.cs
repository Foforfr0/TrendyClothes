using Backend.DTO;
using Backend.DTO.User.Profile;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Backend.Controllers.User.Profile {
    [ApiController]
    [Route ("api/User/Profile/[controller]")]
    public class ViewMyProfileController : Controller {
        private readonly IProfileService _profileService;

        public ViewMyProfileController (IProfileService profileService) {
            _profileService = profileService;
        }

        [HttpGet ("GetMyData")]
        public async Task<IActionResult> GetMyDataAsync (string username) {
            try {
                string? susername = User.FindFirst (JwtRegisteredClaimNames.Sub).Value;
                /*
                 // Obtener el token JWT de la cookie
        var token = Request.Cookies["YourCookieName"]; // Reemplaza "YourCookieName" con el nombre de tu cookie
        if (!string.IsNullOrEmpty(token))
        {
            // Decodificar el token JWT
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            // Obtener el nombre de usuario desde el token
            Username = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value; // Cambia "username" si tu claim tiene otro nombre
                 */

                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Campo vacío.");
                MessageResponse<MyPersonalInformationDTO> response = await _profileService.GetMyPersonalInformation (username);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null)
                    return NotFound (response.message);
                return Ok (new {
                    response.message,
                    body = new JsonResult (response.dataRetrieved)
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("GetAddresses")]
        public async Task<IActionResult> GetMyAddressAsync ([FromQuery] string? username) {
            try {
                if (string.IsNullOrEmpty (username))
                    username = User.FindFirst ("Sub")?.Value;

                if (string.IsNullOrEmpty (username))
                    return BadRequest ("Campo vacío.");
                MessageResponse<List<AddressDTO>> response = await _profileService.GetAddressAsync (username);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null)
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
