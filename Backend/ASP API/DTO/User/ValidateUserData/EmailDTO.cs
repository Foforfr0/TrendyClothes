using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.ValidateUserData {
    public class EmailDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "El correo es requerido.")]
        [MaxLength (100, ErrorMessage = "El correo electrónico muy largo.")]
        [EmailAddress (ErrorMessage = "Formato de correo inválido.")]
        public required string Email {
            get; set;
        }
    }
}
