using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.ValidateUserData {
    public class PhoneNumberDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "La lada de teléfono es requerida.")]
        [StringLength (5, MinimumLength = 2, ErrorMessage = "La lada debe tener de 1 a 4 números.")]
        [RegularExpression (@"^\+\d{1,4}$", ErrorMessage = "La lada del teléfono es inválida.")]
        public required string AreaCode {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "El número de teléfono es requerido.")]
        [RegularExpression (@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos.")]
        public required string PhoneNumber {
            get; set;
        }
    }
}
