namespace Backend.DTO.Auction
{
    public class AuctionDTO
    {
            public int AuctionId { get; set; }
            public int ProductId { get; set; }
            public decimal? FirstPrice { get; set; }
            public decimal? MinBid { get; set; }
            public decimal? LastPrice { get; set; }
            public int? BuyerId { get; set; }
            public string? Status { get; set; }
    }
}
