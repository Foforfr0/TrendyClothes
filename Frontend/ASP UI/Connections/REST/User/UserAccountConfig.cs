namespace WebPage.Connections.REST.User {
    public class UserAccountConfig {
        public string BaseUrl {
            get; set;
        }
        public string PostAccount {
            get; set;
        }
        public string DeleteAccount {
            get; set;
        }
        public ValidateUserDataEndpoints ValidateUserData {
            get; set;
        }
    }
}
