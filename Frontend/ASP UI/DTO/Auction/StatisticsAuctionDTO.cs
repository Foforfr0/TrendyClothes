namespace WebPage.DTO.Auction {
    public class StatisticsAuctionDTO {
        public required int IdAuction {
            get; set;
        }
        public required int NumberBids {
            get; set;
        }
        public required decimal FirstPrice {
            get; set;
        }
        public required decimal LastPrice {
            get; set;
        }
        public required decimal PercentageGain {
            get; set;
        }
        public string Name {
            get; set;
        }
        public DateTime DateStart {
            get; set;
        }
        public DateTime DateEnd {
            get; set;
        }
        public string Status {
            get; set;
        }
        public string ImageBase64 {
            get; set;
        }
        public string MimeImage {
            get; set;
        }
    }
}
