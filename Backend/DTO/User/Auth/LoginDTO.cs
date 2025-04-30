using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Auth {
    public class LoginDTO {
        public LoginDTO (string Username, string Password) {
            this.Username = Username;
            this.Password = Password;
        }
        public required string Username {
            get; set;
        }
        public required string Password {
            get; set;
        }
    }
}
