namespace WebPage.DTO.Auction
{
    public class AuctionsListDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal LastPrice { get; set; }
        public string ImageBase64 { get; set; } = string.Empty;
        public string MimeImage { get; set; } = "image/jpeg";
    }
}
