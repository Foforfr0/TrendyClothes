namespace AuctionAuctioneerService.Models.Consult {
    public class AuctionDetailsDTO {
        public required int Id {
            get; set;
        }
        public required string Name {
            get; set;
        }
        public required decimal StartingPrice {
            get; set;
        }
        public required DateTime StartDate {
            get; set;
        }
        public DateTime? EndDate {
            get; set;
        }
        public required string SellerUsername {
            get; set;
        }
        public int? BidsCount {
            get; set;
        }
        public required decimal CurrentPrice {
            get; set;
        }
    }
}
