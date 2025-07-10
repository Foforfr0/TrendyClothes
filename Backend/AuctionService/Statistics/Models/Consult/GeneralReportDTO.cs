namespace AuctionStatistics.Models.Consult {
    public class GeneralReportDTO {
        public required int TotalAuctionsCreated {
            get; set;
        }
        public required double AverageBidsPerAuction {
            get; set;
        }
        public required int TotalBids {
            get; set;
        }
        public required int MaxBidsInAuction {
            get; set;
        }
        public required int MinBidsInAuction {
            get; set;
        }
        public decimal? HighestBid {
            get; set;
        }
        public decimal? LowestBid {
            get; set;
        }
        public required TimeSpan AverageAuctionDuration {
            get; set;
        }
        public required TimeSpan LongestAuctionDuration {
            get; set;
        }
        public required TimeSpan ShortestAuctionDuration {
            get; set;
        }
        public required DateTime MostRecentAuction {
            get; set;
        }
        public required DateTime OldestAuction {
            get; set;
        }
        public decimal? HighestAuctionGain {
            get; set;
        }
        public decimal? LowestAuctionGain {
            get; set;
        }
    }
}
