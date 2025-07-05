namespace AuctionParticipantService.Models
{
    public class AuctionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? FirstPrice { get; set; }
        public decimal? Bid { get; set; }
        public decimal? LastPrice { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; } = string.Empty;
        public byte[] Photo { get; set; } = Array.Empty<byte>();
        public string Mime { get; set; } = string.Empty;
    }
}
