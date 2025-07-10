namespace WebPage.DTO.Auction {
    public class GeneralReportDTO {
        public int TotalAuctionsCreated {
            get; set;
        }
        public double AverageBidsPerAuction {
            get; set;
        }
        public int TotalBids {
            get; set;
        }
        public int MaxBidsInAuction {
            get; set;
        }
        public int MinBidsInAuction {
            get; set;
        }

        public decimal HighestBid {
            get; set;
        }
        public decimal LowestBid {
            get; set;
        }
        public TimeSpan AverageAuctionDuration {
            get; set;
        }
        public TimeSpan LongestAuctionDuration {
            get; set;
        }
        public TimeSpan ShortestAuctionDuration {
            get; set;
        }
        public DateTime MostRecentAuction {
            get; set;
        }
        public DateTime OldestAuction {
            get; set;
        }
        public decimal HighestAuctionGain {
            get; set;
        }
        public decimal LowestAuctionGain {
            get; set;
        }
    }
}
