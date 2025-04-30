using Backend.DAO.User.Auth;
using Backend.DTO.User.Auth;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class AuthService : IAuthService {
        private readonly AuthDAO _authDAO;

        public AuthService (AuthDAO authDAO) {
            _authDAO = authDAO;
        }

        public async Task<LoginDTO?> PostLoginAsync (LoginDTO loginDTO) {
            Entities.User? retrievedUser = await _authDAO.PostLoginAsync (loginDTO.Username, loginDTO.Password);
            if (retrievedUser == null)
                return null;
            else {
                loginDTO.Username = retrievedUser.Username;
                loginDTO.Password = "**********";
                return loginDTO;
            }
        }
    }
}
