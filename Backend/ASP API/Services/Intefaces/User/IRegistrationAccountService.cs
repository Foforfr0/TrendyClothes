using Backend.DTO;
using Backend.DTO.User.Registration;

namespace Backend.Services.Intefaces.User {
    public interface IRegistrationAccountService {
        public Task<MessageResponse<bool>> PostUserAsync (RegistrationUserDTO newUserDTO);
    }
}
