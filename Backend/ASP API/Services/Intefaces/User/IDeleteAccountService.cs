using Backend.DTO;

namespace Backend.Services.Intefaces.User {
    public interface IDeleteAccountService {
        public Task<MessageResponse<bool>> DeleteUserAsync (string username);
    }
}
