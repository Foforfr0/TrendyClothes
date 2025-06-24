using ProfileService.Models;

namespace ProfileService.Services.Intefaces {
    public interface IConsultProfileService {
        public Task<MessageResponse<MyPersonalInformationDTO>> GetMyDataInformation (string username);
        public Task<MessageResponse<List<AddressDTO>>> GetAddressesAsync (string username);
    }
}
