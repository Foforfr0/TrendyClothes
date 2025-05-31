using System.ComponentModel.DataAnnotations;

namespace WpfApp.DTO.User.Auth {
    public class EmailDTO {
        public string? username {
            get; set;
        }
        public string? email {
            get; set;
        }
        public bool isCorrect {
            get; set;
        }
    }
}
