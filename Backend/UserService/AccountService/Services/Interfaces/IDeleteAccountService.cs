using AccountService.Models;

namespace AccountService.Services.Intefaces {
    public interface IDeleteAccountService {
        public Task<MessageResponse<bool>> DeleteUserAsync (string username);
    }
}
