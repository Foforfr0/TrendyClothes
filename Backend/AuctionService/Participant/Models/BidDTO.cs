namespace AuctionParticipantService.Models {
    public class BidDTO {
        public int? Id {
            get; set;
        }
        public int AuctionId {
            get; set;
        }
        public int? BuyerId {
            get; set;
        }
        public string? BuyerUsername {
            get; set;
        }
    }
}
