namespace Backend.DTO.User.Profile {
    public class AddressDTO {
        public string? street {
            get; set;
        }
        public string? numberExterior {
            get; set;
        }
        public string? numberInterior {
            get; set;
        }
        public string? neighborhood {
            get; set;
        }
        public string? city {
            get; set;
        }
        public string? postalCode {
            get; set;
        }
        public string? state {
            get; set;
        }
        public string? country {
            get; set;
        }
        public bool? isActive {
            get; set;
        }
    }
}
