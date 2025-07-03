using WpfApp.DTO.User.Profile;

namespace WpfApp.Utilities
{
    public class ProfileResponseWrapper
    {
        public string Message { get; set; }
        public PersonalInformationDTO Body { get; set; }
    }
}
