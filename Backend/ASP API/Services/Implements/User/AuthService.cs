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

        public async Task<MessageResponse<LoginDTO>> ValidateLoginAsync (LoginDTO loginDTO) {
            MessageResponse<Entities.User> response = await _authDAO.ValidateLoginAsync (loginDTO);

            if (response.IsError)
                return MessageResponse<LoginDTO>.Failure (response.Message);
            if (response.DataRetrieved == default)
                return MessageResponse<LoginDTO>.Success ("Credenciales incorrectas.", default);

            loginDTO.Username = (response.DataRetrieved).Username;
            loginDTO.Password = "*********";
            return MessageResponse<LoginDTO>.Success ("Credenciales correctas.", loginDTO);
        }

        public async Task<MessageResponse<EmailDTO>> ValidateEmailUserAsync (EmailDTO emailDTO) {
            MessageResponse<Entities.User> response = await _authDAO.ValidateEmailUserAsync (emailDTO);

            if (response.IsError)
                return MessageResponse<EmailDTO>.Failure (response.Message);
            if (response.DataRetrieved == default)
                return MessageResponse<EmailDTO>.Success ("Email no ligado al usuario.", default);

            emailDTO.Username = (response.DataRetrieved).Username;
            emailDTO.Email = (response.DataRetrieved).Email;
            emailDTO.IsCorrect = true;
            return MessageResponse<EmailDTO>.Success ("Email ligado al usuario.", emailDTO);
        }

        public async Task<MessageResponse<bool>> PostTwoFactorCodeAsync (EmailDTO emailDTO) {
            MessageResponse<bool> response = await _authDAO.PostTwoFactorCodeAsync (emailDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }

        public async Task<MessageResponse<jwtDTO>> ValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO) {
            MessageResponse<jwtDTO> response = await _authDAO.ValidateTwoFactorCodeAsync (codeTwoFactorDTO);

            if (response.IsError)
                return MessageResponse<jwtDTO>.Failure (response.Message);
            if (response.DataRetrieved == default)
                return MessageResponse<jwtDTO>.Success (response.Message, default);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return MessageResponse<jwtDTO>.Success ("Código doble factor correcto.",
                new jwtDTO {
                    Username = response.DataRetrieved.Username,
                    Role = response.DataRetrieved.Role
                });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username) {
            MessageResponse<bool> response = await _authDAO.DeleteTwoFactorCodeAsync (username);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
