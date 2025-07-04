namespace AuctionParticipantService.Models.Consult
{
    public class AuctionFullDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public  decimal? FirstPrice { get; set; }
        public  decimal Bid { get; set; }
        public required decimal LastPrice { get; set; }
        public required DateTime DateStart { get; set; }
        public required DateTime DateEnd { get; set; }
        public required string Description { get; set; }
        public required int ProductId { get; set; }
        public required int StatusId { get; set; }
        public required int SellerId { get; set; }
        public required string StatusName { get; set; }
        public required string SellerName { get; set; }
    }
}
