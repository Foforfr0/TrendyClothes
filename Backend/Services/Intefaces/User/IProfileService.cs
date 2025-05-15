using Backend.DTO;
using Backend.DTO.User.Profile;

namespace Backend.Services.Intefaces.User {
    public interface IProfileService {
        public Task<MessageResponse<MyPersonalInformationDTO>> GetMyPersonalInformation (string username);
        public Task<MessageResponse<List<AddressDTO>>> GetAddressAsync (string username);
    }
}
