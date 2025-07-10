namespace AuctionStatistics.Models.Consult {
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
    }
}
