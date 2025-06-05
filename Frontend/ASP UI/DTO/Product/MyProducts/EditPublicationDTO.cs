using System.ComponentModel.DataAnnotations;

namespace WebPage.DTO.Product.MyProducts {
    public class EditPublicationDTO {
        public int id {
            get; set;
        }

        [Required (AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido.")]
        [StringLength (100, MinimumLength = 10, ErrorMessage = "El nombre debe tener entre 10 y 100 caracteres.")]
        public string? name {
            get; set;
        }

        [Required (ErrorMessage = "El precio es requerido.")]
        [Range (0.01, 999999999999.99, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal? price {
            get; set;
        }

        [Range (0.0, 100.0, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        public decimal? discount {
            get; set;
        }

        // Campo solo de lectura
        public int? numberSold {
            get; set;
        }

        // Campo solo de lectura
        public decimal? averageStars {
            get; set;
        }
        [Required (ErrorMessage = "La descripción es requerida.")]
        [StringLength (1000, MinimumLength = 20, ErrorMessage = "La descripción debe tener entre 20 y 1000 caracteres.")]
        public string? description {
            get; set;
        }

        [Required (ErrorMessage = "El stock disponible es requerido.")]
        [Range (0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int? stockAvailable {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar una categoría.")]
        [Range (1, 11, ErrorMessage = "La categoría seleccionada no es válida.")]
        public int? categoryId {
            get; set;
        }

        public string? category {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar un tipo.")]
        [Range (1, 2, ErrorMessage = "El tipo seleccionado no es válido.")]
        public int? typeId {
            get; set;
        }

        public string? type {
            get; set;
        }

        [Required (ErrorMessage = "Debe seleccionar un estado.")]
        [Range (1, 2, ErrorMessage = "El estado seleccionado no es válido.")]
        public int? statusId {
            get; set;
        }

        public string? status {
            get; set;
        }

        // Imagen y tipo MIME no son campos que se editan directamente desde el formulario
        public string? imageBase64 {
            get; set;
        }

        public string? mimeImage {
            get; set;
        }
    }

}
