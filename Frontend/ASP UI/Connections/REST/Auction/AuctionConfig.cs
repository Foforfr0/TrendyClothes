namespace WebPage.Connections.REST.Auction {
    public class AuctionConfig {
        public string BaseUrl {
            get; set;
        }
        public AuctioneerConfig Auctioneer {
            get; set;
        }
        public ParticipantConfig Participant {
            get; set;
        }
    }
}
