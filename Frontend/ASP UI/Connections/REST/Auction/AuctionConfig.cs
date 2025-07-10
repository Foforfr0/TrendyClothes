namespace WebPage.Connections.REST.Auction {
    public class AuctionConfig {
        public AuctionEndpoints Auction {
            get; set;
        }
        public AuctioneerConfig Auctioneer {
            get; set;
        }
        public ParticipantConfig Participant {
            get; set;
        }
        public StatisticsConfig Statistics {
            get; set;
        }
    }
}
