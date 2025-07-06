namespace WebPage.Connections.REST.Product {
    public class ProductConfig {
        public ProductSellerEndpoints Seller {
            get; set;
        }
        public ProductBuyerEndpoints Buyer {
            get; set;
        }
        public ProductProductEndpoints Product {
            get; set;
        }
    }
}
