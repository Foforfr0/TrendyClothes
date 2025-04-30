using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Auth {
    public class LoginDTO {
        public LoginDTO (string Username, string Password) {
            this.Username = Username;
            this.Password = Password;
        }
        [Required (ErrorMessage = "El usuario es requerido.")]
        public required string Username {
            get; set;
        }
        [Required (ErrorMessage = "La contraseña es requerida.")]
        public required string Password {
            get; set;
        }
    }
}
