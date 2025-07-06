using System.Text.Json.Serialization;

namespace ClienteMAUI.Models.DTO.Auctions
{
    internal class AuctionsListDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string ProductName { get; set; } = "";

        [JsonPropertyName("firstPrice")]
        public decimal FirstPrice { get; set; }

        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }

        [JsonPropertyName("bid")]
        public decimal Bid { get; set; }

        [JsonPropertyName("dateEnd")]
        public DateTime DateEnd { get; set; }

        [JsonPropertyName("photo")]
        public string ImageBase64 { get; set; } = "";

        [JsonPropertyName("mime")]
        public string MimeImage { get; set; } = "";

        // Auxiliares para MAUI
        public ImageSource? ImageSource { get; set; }
        public string FirstPriceText { get; set; } = "";
        public string LastPriceText { get; set; } = "";
        public string BidText { get; set; } = "";
        public string EndDateFormatted { get; set; } = "";
    }
}
