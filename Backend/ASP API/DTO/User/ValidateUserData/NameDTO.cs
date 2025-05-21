using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.ValidateUserData {
    public class NameDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido.")]
        [MaxLength (50, ErrorMessage = "El nombre es muy largo.")]
        [RegularExpression (@"^[A-ZÁÉÍÓÚ][a-záéíóúñ]+$",
            ErrorMessage = "Debe iniciar con mayúscula y solo contener letras.")]
        public required string firstName {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "El apellido paterno es requerido.")]
        [MaxLength (50, ErrorMessage = "El apellido paterno es muy largo.")]
        [RegularExpression (@"^[A-ZÁÉÍÓÚ][a-záéíóúñ]+$",
            ErrorMessage = "Debe iniciar con mayúscula y solo contener letras.")]
        public required string middleName {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "El apellido materno es requerido.")]
        [MaxLength (50, ErrorMessage = "El apellido materno es muy largo.")]
        [RegularExpression (@"^[A-ZÁÉÍÓÚ][a-záéíóúñ]+$",
            ErrorMessage = "Debe iniciar con mayúscula y solo contener letras.")]
        public required string lastName {
            get; set;
        }
    }
}
