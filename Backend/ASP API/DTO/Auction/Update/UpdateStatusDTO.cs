using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.Auction.Update {
    public class UpdateStatusDTO {
        [Required (ErrorMessage = "El ID de la subasta es requerido.")]
        [Range (1, int.MaxValue, ErrorMessage = "El ID de la subasta debe ser un número entero positivo.")]
        public required int Id {
            get; set;
        }
        [Required (ErrorMessage = "El ID del nuevo status es requerido..")]
        [Range (1, 3, ErrorMessage = "El valor del ID debe estar entre 1 y 3.")]
        public int? StatusId {
            get; set;
        }
    }
}
