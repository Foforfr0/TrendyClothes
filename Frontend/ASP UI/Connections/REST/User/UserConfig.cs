using WebPage.Connections.REST.User.Account;
using WebPage.Connections.REST.User.Auth;
using WebPage.Connections.REST.User.Profile;

namespace WebPage.Connections.REST.User {
    public class UserConfig {
        public UserAuthConfig Auth {
            get; set;
        }
        public UserProfileConfig Profile {
            get; set;
        }
        public UserAccountConfig Account {
            get; set;
        }
    }
}
