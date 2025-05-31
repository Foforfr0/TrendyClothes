using System.ComponentModel.DataAnnotations;

namespace WebPage.DTO.User.Auth {
    public class EmailDTO {
        [Required (ErrorMessage = "El correo es requerido.")]
        [EmailAddress (ErrorMessage = "Correo inválido.")]
        public required string email {
            get; set;
        }
    }
}
