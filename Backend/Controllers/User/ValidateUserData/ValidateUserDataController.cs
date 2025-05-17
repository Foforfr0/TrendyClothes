using Backend.DTO;
using Backend.DTO.User.ValidateUserData;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.ValidateUserData {
    [ApiController]
    [Route ("api/User/ValidateUserData/[controller]")]
    public class ValidateUserDataController : ControllerBase {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidateDataService _validateDataService;

        public ValidateUserDataController (IHttpContextAccessor httpContextAccessor, IValidateDataService validateDataService) {
            _contextAccessor = httpContextAccessor;
            _validateDataService = validateDataService;
        }

        [HttpGet ("VerifyExistenceUsername")]
        public async Task<IActionResult> GetExistsUsername ([FromQuery] UsernameDTO usernameDTO) {
            try {
                if (!ModelState.IsValid || string.IsNullOrEmpty (usernameDTO.username))
                    return BadRequest ($"Nombre de usuario no válido. {ModelState}");
                MessageResponse<bool> response = await _validateDataService.VerifyExistsUsername (usernameDTO.username);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved)
                    return Ok (new {
                        exists = true
                    });
                return Ok (new {
                    exists = false
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("VerifyExistenceEmail")]
        public async Task<IActionResult> GetExistsEmail ([FromQuery] EmailDTO emailDTO) {
            try {
                if (!ModelState.IsValid || string.IsNullOrEmpty (emailDTO.email))
                    return BadRequest ($"Email no válido. {ModelState}");
                MessageResponse<bool> response = await _validateDataService.VerifyExistsEmail (emailDTO.email);
                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved)
                    return Ok (new {
                        exists = true
                    });
                return Ok (new {
                    exists = false
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }

        [HttpGet ("VerifyExistencePhoneNumber")]
        public async Task<IActionResult> GetExistsEmail ([FromQuery] PhoneNumberDTO phoneNumberDTO) {
            try {
                if (!ModelState.IsValid || string.IsNullOrEmpty (phoneNumberDTO.areaCode) || string.IsNullOrEmpty (phoneNumberDTO.phoneNumber))
                    return BadRequest ($"Número de teléfono no válido. {ModelState}");
                MessageResponse<bool> response = await _validateDataService.VerifyExistsPhoneNumber (phoneNumberDTO.areaCode, phoneNumberDTO.phoneNumber);
                if(response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved)
                    return Ok (new {
                        exists = true
                    });
                return Ok (new {
                    exists = false
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
