namespace WebPage.DTO.Auction {
    public class MyAuctionDetailsDTO {
        public int Id {
            get; set;
        }
        public string Name {
            get; set;
        }
        public decimal FirstPrice {
            get; set;
        }
        public decimal Bid {
            get; set;
        }
        public int BidsCount {
            get; set;
        }
        public decimal LastPrice {
            get; set;
        }
        public DateTime DateStart {
            get; set;
        }
        public DateTime DateEnd {
            get; set;
        }
        public int StatusId {
            get; set;
        }
        public string Status {
            get; set;
        }
        public string Description {
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
