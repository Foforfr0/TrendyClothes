namespace Backend.DTO.User.Profile {
    public class AddressDTO {
        public string? Street {
            get; set;
        }
        public string? NumberExterior {
            get; set;
        }
        public string? NumberInterior {
            get; set;
        }
        public string? Neighborhood {
            get; set;
        }
        public string? City {
            get; set;
        }
        public string? PostalCode {
            get; set;
        }
        public string? State {
            get; set;
        }
        public string? Country {
            get; set;
        }
        public bool? IsActive {
            get; set;
        }
    }
}
