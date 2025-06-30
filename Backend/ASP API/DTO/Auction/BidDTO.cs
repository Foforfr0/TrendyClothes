namespace Backend.DTO.Auction
{
    public class BidDTO
    {
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
