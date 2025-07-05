using System.ComponentModel.DataAnnotations;

namespace WebPage.DTO.Auction {
    public class CreateAuctionDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido.")]
        [Length (20, 100, ErrorMessage = "El nombre debe contener entre 20 y 100 caracteres.")]
        public string? Name {
            get; set;
        }

        [Range (0.00, double.MaxValue, ErrorMessage = "El precio inicial debe ser un valor positivo.")]
        public decimal? FirstPrice {
            get; set;
        }

        [Required (ErrorMessage = "El valor de la puja es requerido.")]
        [Range (0.01, double.MaxValue, ErrorMessage = "El incremento de puja debe ser mayor a 0.")]
        public decimal? Bid {
            get; set;
        }

        [Required (ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime? DateStart {
            get; set;
        }

        [Required (ErrorMessage = "La fecha de finalización es obligatoria.")]
        public DateTime? DateEnd {
            get; set;
        }

        [Required (AllowEmptyStrings = false, ErrorMessage = "La descripción es obligatoria.")]
        [Length (20, 200, ErrorMessage = "La descripción debe contener entre 20 y 200 caracteres.")]
        public string? Description {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar una status de la subasta.")]
        [Range (1, 3, ErrorMessage = "El status seleccionada no es válida.")]
        public int? StatusId {
            get; set;
        }

        public string? SellerUsername {
            get; set;
        }

        public string? imageBase64 {
            get; set;
        }

        public string? mimeImage {
            get; set;
        }
    }
}
