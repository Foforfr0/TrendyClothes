using System.ComponentModel.DataAnnotations;

namespace AuthService.Models {
    public class LoginDTO {
        [Required (ErrorMessage = "El nombre de usuario es requerido.")]
        public required string Username {
            get; set;
        }
        [Required (ErrorMessage = "La contraseña es requerida.")]
        public required string Password {
            get; set;
        }
    }
}
