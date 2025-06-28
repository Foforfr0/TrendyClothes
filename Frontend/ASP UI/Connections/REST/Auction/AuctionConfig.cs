using WebPage.Connections.REST.Auction.Auctioneer;
using WebPage.Connections.REST.Auction.Participant;

namespace WebPage.Connections.REST.Auction {
    public class AuctionConfig {
        public AuctioneerConfig Auctioneer {
            get; set;
        }
        public ParticipantConfig Participant {
            get; set;
        }
    }
}
