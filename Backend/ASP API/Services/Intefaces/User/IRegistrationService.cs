using Backend.DTO;
using Backend.DTO.User.Registration;

namespace Backend.Services.Intefaces.User {
    public interface IRegistrationService {
        public Task<MessageResponse<bool>> PostUserAsync (RegistrationUserDTO newUserDTO);
        public Task<MessageResponse<bool>> DeleteUserAsync (string username);
    }
}
