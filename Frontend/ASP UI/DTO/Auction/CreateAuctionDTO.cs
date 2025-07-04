namespace WebPage.DTO.Auction {
    public class CreateAuctionDTO {
        public string Name {
            get; set;
        }
        public decimal FirstPrice {
            get; set;
        }
        public decimal Bid {
            get; set;
        }
        public DateTime DateStart {
            get; set;
        }
        public DateTime DateEnd {
            get; set;
        }
        public string Description {
            get; set;
        }
        public string SellerUsername {
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
