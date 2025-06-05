namespace Backend.DTO.Product.Consult {
    public class ProductDetailsDTO {
        public required int Id {
            get; set;
        }
        public required string Name {
            get; set;
        }
        public required decimal Price {
            get; set;
        }
        public required decimal Discount {
            get; set;
        }
        public required int NumberSold {
            get; set;
        }
        public required decimal AverageStars {
            get; set;
        }
        public required string Description {
            get; set;
        }
        public required int StockAvailable {
            get; set;
        }
        public required string SellerUsername {
            get; set;
        }
        public required string Category {
            get; set;
        }
        public required string Type {
            get; set;
        }
        public required int StatusId {
            get; set;
        }
        public required string Status {
            get; set;
        }
    }
}
