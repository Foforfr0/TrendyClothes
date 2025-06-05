namespace WebPage.Connections.REST.Product {
    public class ProductConfig {
        public string BaseUrl {
            get; set;
        }
        public ProductSellerEndpoints Seller {
            get; set;
        }
        public ProductBuyerEndpoints Buyer {
            get; set;
        }
        public ProductTagsEndpoints Tags {
            get; set;
        }
    }
}
