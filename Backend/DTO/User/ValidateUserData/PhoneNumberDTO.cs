using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.ValidateUserData {
    public class PhoneNumberDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "La lada número de teléfono es requerida.")]
        [RegularExpression (@"^\+[0-9]{1,4}$", ErrorMessage = "La lada dell teléfono es inválida.")]
        public required string areaCode {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "El número de teléfono es requerido.")]
        [Phone (ErrorMessage = "El formato del número de teléfono es inválido.")]
        public required string phoneNumber {
            get; set;
        }
    }
}
