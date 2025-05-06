namespace Backend.DTO.User.Auth {
    public class EmailDTO {
        public EmailDTO () {
            
        }
        public required string username {
            get; set;
        }
        public required string email {
            get; set;
        }
        public bool isCorrect {
            get; set;
        }
    }
}
