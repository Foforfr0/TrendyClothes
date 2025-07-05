using AccountService.Models;
using AccountService.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers {
    [ApiController]
    [Route ("api/User/ValidateUserData")]
    public class ValidateUserDataController : ControllerBase {
        private readonly IValidateDataService _validateDataService;

        public ValidateUserDataController (IValidateDataService validateDataService) {
            _validateDataService = validateDataService;
        }

        [HttpGet ("VerifyExistenceUsername")]
        public async Task<IActionResult> GetExistsUsername ([FromQuery] UsernameDTO usernameDTO) {
            try {
                if (!ModelState.IsValid || string.IsNullOrEmpty (usernameDTO.Username))
                    return BadRequest ($"Nombre de usuario no válido. {ModelState}");
                MessageResponse<bool> response = await _validateDataService.VerifyExistsUsername (usernameDTO.Username);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved)
                    return Ok (new {
                        exists = true
                    });
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("VerifyExistenceEmail")]
        public async Task<IActionResult> GetExistsEmail ([FromQuery] EmailDTO emailDTO) {
            try {
                if (!ModelState.IsValid || string.IsNullOrEmpty (emailDTO.Email))
                    return BadRequest ($"Email no válido. {ModelState}");
                MessageResponse<bool> response = await _validateDataService.VerifyExistsEmail (emailDTO.Email);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved)
                    return Ok (new {
                        exists = true
                    });
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("VerifyExistencePhoneNumber")]
        public async Task<IActionResult> GetExistsPhoneNumber ([FromQuery] PhoneNumberDTO phoneNumberDTO) {
            try {
                if (!ModelState.IsValid || string.IsNullOrEmpty (phoneNumberDTO.AreaCode) || string.IsNullOrEmpty (phoneNumberDTO.PhoneNumber))
                    return BadRequest ($"Número de teléfono no válido. {ModelState}");
                MessageResponse<bool> response = await _validateDataService.VerifyExistsPhoneNumber (phoneNumberDTO.AreaCode, phoneNumberDTO.PhoneNumber);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved)
                    return Ok (new {
                        exists = true
                    });
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
