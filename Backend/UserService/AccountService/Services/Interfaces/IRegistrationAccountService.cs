using AccountService.Models;

namespace AccountService.Services.Intefaces {
    public interface IRegistrationAccountService {
        public Task<MessageResponse<bool>> PostUserAsync (RegistrationUserDTO newUserDTO);
    }
}
