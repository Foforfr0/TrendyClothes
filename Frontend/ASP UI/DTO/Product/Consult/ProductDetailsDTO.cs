namespace WebPage.DTO.Product.Consult {
    public class ProductDetailsDTO {
        public int id {
            get; set;
        }
        public string name {
            get; set;
        }
        public decimal price {
            get; set;
        }
        public decimal? discount {
            get; set;
        }
        public int numberSold {
            get; set;
        }
        public decimal? averageStars {
            get; set;
        }
        public string description {
            get; set;
        }
        public int stockAvailable {
            get; set;
        }
        public int sellerId {
            get; set;
        }
        public string sellerName {
            get; set;
        }
        public string category {
            get; set;
        }
        public string type {
            get; set;
        }
        public int idStatus {
            get; set;
        }
        public string status {
            get; set;
        }
        public string? imageBase64 {
            get; set;
        }
        public string? mimeImage {
            get; set;
        }
    }
}
