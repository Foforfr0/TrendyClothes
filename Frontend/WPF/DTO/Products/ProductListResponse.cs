using System.Text.Json.Serialization;

namespace WpfApp.DTO.Products
{
    public class ProductListResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = "";

        [JsonPropertyName("body")]
        public List<ProductoAPIModel> Body { get; set; } = new();
    }

    public class ProductoAPIModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("discount")]
        public decimal discount { get; set; }

        [JsonPropertyName("numberSold")]
        public int NumberSold { get; set; }

        [JsonPropertyName("averageStars")]
        public float AverageStars { get; set; }

        [JsonPropertyName("stockAvailable")]
        public int StockAvailable { get; set; }
    }
}
