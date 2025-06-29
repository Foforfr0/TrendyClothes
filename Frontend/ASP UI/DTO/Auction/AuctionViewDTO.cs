namespace WebPage.DTO.Auction
{
    public class AuctionViewDTO
    {
        public int AuctionId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal? FirstPrice { get; set; }
        public decimal? MinBid { get; set; }
        public decimal? LastPrice { get; set; }
        public string? mimeImage { get; set; }
        public string? imageBase64 { get; set; }
    }
}
