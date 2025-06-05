using WebPage.Connections.REST.Auction;
using WebPage.Connections.REST.Product;
using WebPage.Connections.REST.User;

namespace WebPage.Connections.REST {
    public class RestConfig {
        public UserConfig User {
            get; set;
        }
        public ProductConfig Product {
            get; set;
        }
        public AuctionConfig Auction {
            get; set;
        }
    }
}
