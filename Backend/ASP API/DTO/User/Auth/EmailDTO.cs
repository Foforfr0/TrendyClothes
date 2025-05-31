using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Auth {
    public class EmailDTO {
        [Required (ErrorMessage = "El nombre de usuario es requerido.")]
        public required string username {
            get; set;
        }
        [Required (ErrorMessage = "El correo es requerido.")]
        [EmailAddress (ErrorMessage = "Formato de correo inválido.")]
        public required string email {
            get; set;
        }
        public bool isCorrect {
            get; set;
        }
    }
}
