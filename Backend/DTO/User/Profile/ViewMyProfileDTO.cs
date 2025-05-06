namespace Backend.DTO.User.Profile {
    public class ViewMyProfileDTO {
        public ViewMyProfileDTO () {

        }
        public required string firstName {
            get; set;
        }
        public required string middleName {
            get; set;
        }
        public required string lastName {
            get; set;
        }
        public required string username {
            get; set;
        }
        public required string email {
            get; set;
        }
    }
}
