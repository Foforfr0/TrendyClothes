using System.ComponentModel.DataAnnotations;

namespace ProductSellerService.Models {
    public class NewProductDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido.")]
        [StringLength (100, MinimumLength = 10, ErrorMessage = "El nombre debe tener entre 10 y 100 caracteres.")]
        public required string Name {
            get; set;
        }

        [Required (ErrorMessage = "El precio es requerido.")]
        [Range (0.01, 999999999999.99, ErrorMessage = "El precio debe ser mayor que 0.")]
        public required decimal Price {
            get; set;
        }

        [Range (0.0, 100.0, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        public decimal Discount {
            get; set;
        }

        [Required (ErrorMessage = "La descripción es requerida.")]
        [StringLength (1000, MinimumLength = 20, ErrorMessage = "La descripción debe tener entre 20 y 1000 caracteres.")]
        public required string Description {
            get; set;
        }

        [Required (ErrorMessage = "El stock disponible es requerido.")]
        [Range (0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public required int StockAvailable {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar una categoría.")]
        [Range (1, 11, ErrorMessage = "La categoría seleccionada no es válida.")]
        public required int CategoryId {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar un tipo.")]
        [Range (1, 2, ErrorMessage = "El tipo seleccionado no es válido.")]
        public required int TypeId {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar un estado.")]
        [Range (1, 2, ErrorMessage = "El estado seleccionado no es válido.")]
        public required int StatusId {
            get; set;
        }

        public string? UsernameSeller {
            get; set;
        }

        public int SellerId {
            get; set;
        }
    }
}
