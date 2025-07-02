using System.ComponentModel.DataAnnotations;

namespace WebPage.DTO.Product.MyProducts {
    public class NewProductDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido.")]
        [StringLength (100, MinimumLength = 10, ErrorMessage = "El nombre debe tener entre 10 y 100 caracteres.")]
        public required string name {
            get; set;
        }

        [Required (ErrorMessage = "El precio es requerido.")]
        [Range (0.01, 999999999999.99, ErrorMessage = "El precio debe ser mayor que 0.")]
        public required decimal price {
            get; set;
        }

        [Range (0.0, 100.0, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        public decimal discount {
            get; set;
        }

        [Required (ErrorMessage = "La descripción es requerida.")]
        [StringLength (1000, MinimumLength = 20, ErrorMessage = "La descripción debe tener entre 20 y 1000 caracteres.")]
        public required string description {
            get; set;
        }

        [Required (ErrorMessage = "El stock disponible es requerido.")]
        [Range (0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public required int stockAvailable {
            get; set;
        }

        [Required (ErrorMessage = "ID del vendedor es requerido.")]
        [Range (1, int.MaxValue, ErrorMessage = "El ID del vendedor debe ser número positivo.")]
        public required int sellerId {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar una categoría.")]
        [Range (1, 11, ErrorMessage = "La categoría seleccionada no es válida.")]
        public required int categoryId {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar un tipo.")]
        [Range (1, 2, ErrorMessage = "El tipo seleccionado no es válido.")]
        public required int typeId {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar un estado.")]
        [Range (1, 2, ErrorMessage = "El estado seleccionado no es válido.")]
        public required int statusId {
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
