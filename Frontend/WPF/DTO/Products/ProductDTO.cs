using System.Text.Json.Serialization;

namespace WpfApp.DTO.Products
{
    public class ProductDTO
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("discount")]
        public float Discount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("stockAvailable")]
        public int StockAvailable { get; set; }

        [JsonPropertyName("sellerUsername")]
        public string UsernameSeller { get; set; } = string.Empty;

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("typeId")]
        public int TypeId { get; set; }

        [JsonPropertyName("category")]
        public string? CategoryName { get; set; }

        [JsonPropertyName("type")]
        public string? TypeName { get; set; }

        [JsonPropertyName("statusId")]
        public int StatusId { get; set; }
    }
}
