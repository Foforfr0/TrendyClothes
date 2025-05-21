using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Auth {
    public class CodeTwoFactorDTO {
        [Required (ErrorMessage = "El nombre de usuario es requerido.")]
        public required string username {
            get; set;
        }
        [Required (ErrorMessage = "El código doble factor es requerido.")]
        [StringLength (maximumLength: 6, MinimumLength = 6, ErrorMessage = "El código debe contener 6 dígitos.")]
        public required string twoFactorCode {
            get; set;
        }
    }
}
