using Backend.DTO.User.Auth;

namespace Backend.Services.Intefaces.User {
    public interface IAuthService {
        public Task<LoginDTO?> PostLoginAsync (LoginDTO login);
    }
}
