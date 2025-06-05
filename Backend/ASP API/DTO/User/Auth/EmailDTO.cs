using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Auth {
    public class EmailDTO {
        [Required (ErrorMessage = "El nombre de usuario es requerido.")]
        public required string Username {
            get; set;
        }
        [Required (ErrorMessage = "El correo es requerido.")]
        [EmailAddress (ErrorMessage = "Formato de correo inválido.")]
        public required string Email {
            get; set;
        }
        public bool IsCorrect {
            get; set;
        }
    }
}
