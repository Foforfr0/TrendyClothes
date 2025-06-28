using System.ComponentModel.DataAnnotations;

namespace AuctionAuctioneerService.Models.Update {
    public class UpdateLastPriceDTO {
        [Required (ErrorMessage = "El ID de la subasta es requerido.")]
        [Range (1, int.MaxValue, ErrorMessage = "El ID de la subasta debe ser un número entero positivo.")]
        public required int Id {
            get; set;
        }
        [Required (ErrorMessage = "El valor del nuevo precio alcanzado es requerido.")]
        [Range (0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public required decimal LastPrice {
            get; set;
        }
    }
}
