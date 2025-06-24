namespace ProductBuyerService.Models {
    public class ProductsDTO {
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
        public required int StockAvailable {
            get; set;
        }
    }
}
