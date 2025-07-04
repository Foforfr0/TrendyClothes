namespace AuctionParticipantService.Models
{
    public class AuctionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal? FirstPrice { get; set; }
        public decimal? Bid { get; set; }
        public decimal? LastPrice { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Description { get; set; } = null!;
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int StatusId { get; set; }
    }
}
