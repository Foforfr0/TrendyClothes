namespace AuctionParticipantService.Models.Consult
{
    public class AuctionsListDTO
    {
            public required int Id { get; set; }
            public required string ProductName { get; set; }
            public required decimal LastPrice { get; set; }
            public required string ImageBase64 { get; set; }
            public required string MimeImage { get; set; }
    }
}
