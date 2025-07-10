namespace ClienteMAUI.Models.DTO.Auctions
{
    public class BidHistoryEntry
    {
        public string Username { get; set; } = "";
        public decimal AmountBidded { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public static class BidHistoryStore
    {
        // auctionId → list of bids
        private static readonly Dictionary<int, List<BidHistoryEntry>> BidHistoryPerAuction = new();

        public static void AddBid(int auctionId, BidHistoryEntry entry)
        {
            if (!BidHistoryPerAuction.ContainsKey(auctionId))
                BidHistoryPerAuction[auctionId] = new List<BidHistoryEntry>();

            BidHistoryPerAuction[auctionId].Add(entry);
        }

        public static List<BidHistoryEntry> GetBids(int auctionId)
        {
            return BidHistoryPerAuction.ContainsKey(auctionId)
                ? BidHistoryPerAuction[auctionId]
                : new List<BidHistoryEntry>();
        }
    }

}