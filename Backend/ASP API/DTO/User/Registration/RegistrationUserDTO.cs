using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.Registration {
    public class RegistrationUserDTO {
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
        [Required (AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es requerido.")]
        [StringLength (40, MinimumLength = 5, ErrorMessage = "El nombre de usuario debe tener entre 5 y 40 caracteres..")]
        [RegularExpression (@"^[a-zA-Z0-9]{5,40}$",
            ErrorMessage = "El nombre de usuario puede contener letras y números.")]
        public required string username {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "El correo es requerido.")]
        [MaxLength (100, ErrorMessage = "El correo electrónico muy largo.")]
        [EmailAddress (ErrorMessage = "Formato de correo inválido.")]
        public required string email {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "La lada de teléfono es requerida.")]
        [StringLength (5, MinimumLength = 2, ErrorMessage = "La lada debe tener de 1 a 4 números.")]
        [RegularExpression (@"^\+\d{1,4}$", ErrorMessage = "La lada del teléfono es inválida.")]
        public required string areaCode {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "El número de teléfono es requerido.")]
        [RegularExpression (@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos.")]
        [Phone (ErrorMessage = "El número de teléfono es inválido.")]
        public required string phoneNumber {
            get; set;
        }
        [Required (AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerido.")]
        [StringLength (200, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,200}$",
            ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y un carácter especial.")]
        public required string password {
            get; set;
        }
        /*
        [Required (ErrorMessage = "El rol del usuario es requerido.")]
        [RegularExpression (@"^[1-2]{1}$",
            ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y un carácter especial.")]
        public required int roleId {
            get; set;
        }
        */
    }
}
