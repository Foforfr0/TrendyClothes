using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.ValidateUserData {
    public class EmailDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "El correo es requerido.")]
        [EmailAddress (ErrorMessage = "Formato de correo inválido.")]
        [MaxLength (100, ErrorMessage = "El correo electrónico muy largo.")]
        public required string email {
            get; set;
        }
    }
}
