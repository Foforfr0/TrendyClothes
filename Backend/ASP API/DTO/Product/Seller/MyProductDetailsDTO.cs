namespace Backend.DTO.Product.MyProducts {
    public class MyProductDetailsDTO {
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
        public required int CategoryId {
            get; set;
        }
        public required string Category {
            get; set;
        }
        public required int TypeId {
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
