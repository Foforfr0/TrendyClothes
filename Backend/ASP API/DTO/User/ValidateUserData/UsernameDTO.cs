using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.User.ValidateUserData {
    public class UsernameDTO {
        [Required (AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es requerido.")]
        [StringLength (40, MinimumLength = 5, ErrorMessage = "El nombre de usuario debe tener entre 5 y 40 caracteres..")]
        [RegularExpression (@"^[a-zA-Z0-9]{5,40}$",
            ErrorMessage = "El nombre de usuario puede contener letras y números.")]
        public required string Username {
            get; set;
        }
    }
}
