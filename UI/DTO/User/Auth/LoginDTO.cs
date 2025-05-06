using System.ComponentModel.DataAnnotations;

namespace UI.DTO.User.Auth {
    public class LoginDTO {
        [Required (ErrorMessage = "El usuario es requerido.")]
        public required string username {
            get; set;
        }
        [Required (ErrorMessage = "La contraseña es requerida.")]
        public required string password {
            get; set;
        }
    }
}
