namespace WebPage.Connections.REST.User {
    public class LoginEndpoints {
        public string Login {
            get; set;
        }
        public string ValidateEmailUser {
            get; set;
        }
        public string PostTwoFactorCode {
            get; set;
        }
        public string ValidateTwoFactorCode {
            get; set;
        }
        public string DeleteTwoFactorCode {
            get; set;
        }
    }
}
