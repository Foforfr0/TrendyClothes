namespace Backend.DTO.User.Auth {
    public class CodeTwoFactorDTO {
        public CodeTwoFactorDTO () {

        }
        public required string username {
            get; set;
        }
        public required string twoFactorCode {
            get; set;
        }
    }
}
