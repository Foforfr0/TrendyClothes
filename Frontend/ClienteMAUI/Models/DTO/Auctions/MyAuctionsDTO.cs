using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClienteMAUI.Models.DTO.Auctions
{
    public class MyAuctionsDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("firstPrice")]
        public decimal FirstPrice { get; set; }

        [JsonPropertyName("dateStart")]
        public DateTime DateStart { get; set; }

        [JsonPropertyName("dateEnd")]
        public DateTime DateEnd { get; set; }

        [JsonPropertyName("bidsCount")]
        public int? BidsCount { get; set; }

        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("imageBase64")]
        public string ImageBase64 { get; set; } = "";

        [JsonPropertyName("mimeImage")]
        public string MimeImage { get; set; } = "";

        [JsonIgnore]
        public ImageSource? ImageSource { get; set; }

        [JsonIgnore]
        public string FirstPriceText => $"${FirstPrice:N2}";

        [JsonIgnore]
        public string LastPriceText => $"${LastPrice:N2}";

        [JsonIgnore]
        public string DateStartText => DateStart.ToString("dd/MM/yyyy HH:mm");

        [JsonIgnore]
        public string DateEndText => DateEnd.ToString("dd/MM/yyyy HH:mm");

    }
}
