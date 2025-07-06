namespace WebPage.Connections.REST.User {
    public class UserAuthConfig {
        public string BaseUrl {
            get; set;
        }
        public LoginEndpoints Login {
            get; set;
        }
        public string Logout {
            get; set;
        }
    }
}
