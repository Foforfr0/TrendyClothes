using System.ComponentModel.DataAnnotations;

namespace UI.DTO.User.Auth {
    public class CodeTwoFactorDTO {
        [Required (ErrorMessage = "Favor de ingresar el código enviado.")]
        [StringLength (maximumLength: 6, MinimumLength = 6, ErrorMessage = "El código debe contener 6 dígitos.")]
        public required string twoFactorCode {
            get; set;
        }
    }
}
