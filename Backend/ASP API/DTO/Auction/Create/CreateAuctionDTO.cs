namespace Backend.DTO.Auction.Create {
    public class CreateAuctionDTO {
        public required string Name {
            get; set;
        }
        public decimal? FirstPrice {
            get; set;
        }
        public decimal? MinBid {
            get; set;
        }
        public required DateTime DateStart {
            get; set;
        }
        public required DateTime DateEnd {
            get; set;
        }
        public required int ProductId {
            get; set;
        }
        public required int NumberProducts {
            get; set;
        }
        public required int SellerId {
            get; set;
        }
    }
}
