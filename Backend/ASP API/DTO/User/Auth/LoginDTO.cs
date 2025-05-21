using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Auth {
    public class LoginDTO {
        [Required (ErrorMessage = "El nombre de usuario es requerido.")]
        public required string username {
            get; set;
        }
        [Required (ErrorMessage = "La contraseña es requerida.")]
        public required string password {
            get; set;
        }
    }
}
