using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMAUI.Models.DTO.Pruducts
{
    public class ProductDTO
    {
        public int? Id { get; set; } // Usado solo al editar
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public float Discount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int StockAvailable { get; set; }
        public string UsernameSeller { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
    }
}
