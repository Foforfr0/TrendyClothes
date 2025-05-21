using Backend.DAO.User;
using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class AuthService : IAuthService {
        private readonly AuthDAO _authDAO;

        public AuthService (AuthDAO authDAO) {
            _authDAO = authDAO;
        }

        public async Task<MessageResponse<LoginDTO>> PostLoginAsync (LoginDTO loginDTO) {
            MessageResponse<Entities.User> response = await _authDAO.ValidateLoginAsync (loginDTO);

            if (response.isError)
                return MessageResponse<LoginDTO>.Failure (response.message);
            if (response.dataRetrieved == default)
                return MessageResponse<LoginDTO>.Success ("Credenciales incorrectas.", default);

            loginDTO.username = (response.dataRetrieved).Username;
            loginDTO.password = "*********";
            return MessageResponse<LoginDTO>.Success ("Credenciales correctas.", loginDTO);
        }

        public async Task<MessageResponse<EmailDTO>> GetValidateEmailUserAsync (EmailDTO emailDTO) {
            MessageResponse<Entities.User> response = await _authDAO.ValidateEmailUserAsync (emailDTO);

            if (response.isError)
                return MessageResponse<EmailDTO>.Failure (response.message);
            if (response.dataRetrieved == default)
                return MessageResponse<EmailDTO>.Success ("Email no ligado al usuario.", default);

            emailDTO.username = (response.dataRetrieved).Username;
            emailDTO.email = (response.dataRetrieved).Email;
            emailDTO.isCorrect = true;
            return MessageResponse<EmailDTO>.Success ("Email ligado al usuario.", emailDTO);
        }

        public async Task<MessageResponse<bool>> PostTwoFactorCodeAsync (EmailDTO emailDTO) {
            MessageResponse<bool> response = await _authDAO.PostTwoFactorCodeAsync (emailDTO);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success (response.message, false);
            return MessageResponse<bool>.Success (response.message, true);
        }

        public async Task<MessageResponse<jwtDTO>> GetValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO) {
            MessageResponse<jwtDTO> response = await _authDAO.ValidateTwoFactorCodeAsync (codeTwoFactorDTO);

            if (response.isError)
                return MessageResponse<jwtDTO>.Failure (response.message);
            if (response.dataRetrieved == default)
                return MessageResponse<jwtDTO>.Success (response.message, default);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return MessageResponse<jwtDTO>.Success ("Código doble factor correcto.",
                new jwtDTO {
                    username = response.dataRetrieved.username,
                    role = response.dataRetrieved.role
                });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username) {
            MessageResponse<bool> response = await _authDAO.DeleteTwoFactorCodeAsync (username);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success (response.message, false);
            return MessageResponse<bool>.Success (response.message, true);
        }
    }
}
