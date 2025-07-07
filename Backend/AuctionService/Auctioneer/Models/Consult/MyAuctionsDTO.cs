namespace AuctionAuctioneerService.Models.Consult {
    public class MyAuctionsDTO {
        public required int Id {
            get; set;
        }
        public required string Name {
            get; set;
        }
        public required decimal FirstPrice {
            get; set;
        }
        public required DateTime DateStart {
            get; set;
        }
        public required DateTime DateEnd {
            get; set;
        }
        public int? BidsCount {
            get; set;
        }
        public required decimal LastPrice {
            get; set;
        }
        public required string Status {
            get; set;
        }
        public required string ImageBase64 {
            get; set;
        }
        public required string MimeImage {
            get; set;
        }
    }
}
