using Backend.DTO;
using Backend.DTO.User.Profile;

namespace Backend.Services.Intefaces.User {
    public interface IProfileService {
        public Task<MessageResponse<MyPersonalInformationDTO>> GetMyDataInformation (string username);
        public Task<MessageResponse<List<AddressDTO>>> GetAddressesAsync (string username);
    }
}
