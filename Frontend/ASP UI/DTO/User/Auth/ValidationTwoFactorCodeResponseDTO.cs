namespace WebPage.DTO.User.Auth {
    public class ValidationTwoFactorCodeResponseDTO {
        public string message {
            get; set;
        }
        public string jwtToken {
            get; set;
        }
    }
}
