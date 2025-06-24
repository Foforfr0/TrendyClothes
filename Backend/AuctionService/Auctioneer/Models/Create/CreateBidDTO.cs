using System.ComponentModel.DataAnnotations;

namespace AuctionAuctioneerService.Models.Create {
    public class CreateBidDTO {
        [Required (ErrorMessage = "La cantidad de la puja es requerida.")]
        [Range (0.01, double.MaxValue, ErrorMessage = "La cantidad de la puja debe ser mayor que cero.")]
        public required decimal Bid {
            get; set;
        }
        [Required (ErrorMessage = "La fecha/hora de la puja es requerida.")]
        [DataType (DataType.DateTime, ErrorMessage = "La fecha/hora de la puja debe ser una fecha y hora válida.")]
        public required DateTime DateBid {
            get; set;
        }
        [Required (ErrorMessage = "El ID del comprador es requerido.")]
        [Range (1, int.MaxValue, ErrorMessage = "El ID del comprador debe ser un número positivo.")]
        public required int BuyerId {
            get; set;
        }
        [Required (ErrorMessage = "El ID de la subasta es requerido.")]
        [Range (1, int.MaxValue, ErrorMessage = "El ID de la subasta debe ser un número positivo.")]
        public required int AuctionId {
            get; set;
        }
    }
}
