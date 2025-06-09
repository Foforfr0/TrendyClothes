namespace WebPage.DTO.Auction {
    public class CreateAuctionDTO {
        public int IdProduct {
            get; set;
        }
        public string Name {
            get; set;
        }
        public decimal FirstPrice {
            get; set;
        }
        public decimal MinBid {
            get; set;
        }
        public DateTime DateStart {
            get; set;
        }
        public DateTime DateEnd {
            get; set;
        }
        public int NumberProducts {
            get; set;
        }
    }
}
