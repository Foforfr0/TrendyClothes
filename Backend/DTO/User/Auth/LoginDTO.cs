using System.Collections;

namespace Backend.DTO.User.Auth {
    public class LoginDTO {
        public LoginDTO () {
            
        }
        public required string username {
            get; set;
        }
        public required string password {
            get; set;
        }
    }
}
