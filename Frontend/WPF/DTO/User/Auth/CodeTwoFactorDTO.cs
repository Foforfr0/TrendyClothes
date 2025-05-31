using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Auth {
    public class CodeTwoFactorDTO {
        public string? username {
            get; set;
        }
        public string? twoFactorCode {
            get; set;
        }
    }
}
