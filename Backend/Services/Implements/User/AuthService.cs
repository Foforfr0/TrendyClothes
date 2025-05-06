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
            MessageResponse<Entities.User> response = await _authDAO.ValidateLogin (loginDTO);

            if (response.isError)
                return MessageResponse<LoginDTO>.Failure (response.message);
            if (response.dataRetrieved == null)
                return MessageResponse<LoginDTO>.Success ("Credenciales incorrectas.", default);

            loginDTO.username = (response.dataRetrieved).Username;
            loginDTO.password = "*********";
            return MessageResponse<LoginDTO>.Success ("Usuario encontrado.", loginDTO);
        }

        public async Task<MessageResponse<EmailDTO>> GetValidateEmailUserAsync (EmailDTO emailDTO) {
            MessageResponse<Entities.User> response = await _authDAO.ValidateEmailUser (emailDTO);

            if (response.isError)
                return MessageResponse<EmailDTO>.Failure (response.message);
            if (response.dataRetrieved == null)
                return MessageResponse<EmailDTO>.Success ("Email no ligado al usuario.", null);

            emailDTO.username = (response.dataRetrieved).Username;
            emailDTO.email = (response.dataRetrieved).Email;
            emailDTO.isCorrect = true;
            return MessageResponse<EmailDTO>.Success ("Email ligado al usuario.", emailDTO);
        }

        public async Task<MessageResponse<bool>> PostTwoFactorCodeAsync (string username) {
            MessageResponse<bool> response = await _authDAO.CreateTwoFactorCode (username);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success ("Usuario no encontrado.", false);
            return MessageResponse<bool>.Success ("Código doble factor creado.", true);
        }

        public async Task<MessageResponse<bool>> GetValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO) {
            MessageResponse<Entities.User> response = await _authDAO.ValidateTwoFactorCode (codeTwoFactorDTO);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == null && response.message.Equals ("User not found"))
                return MessageResponse<bool>.Success ("Usuario no encontrado.", false);
            if (response.dataRetrieved == null && response.message.Equals ("User doesn't have twoFactorCode."))
                return MessageResponse<bool>.Success ("Usuario no posee un código doble factor.", false);
            if (response.dataRetrieved == null && response.message.Equals ("TwoFactorCode incorrect."))
                return MessageResponse<bool>.Success ("Código doble factor incorrecto.", false);
            if (response.dataRetrieved != null && response.message.Equals ("TwoFactorCode correct."))
                return MessageResponse<bool>.Success ("Código doble factor correcto.", true);
            else
                return MessageResponse<bool>.Success ("Código doble factor correcto.", true);
        }

        public async Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username) {
            MessageResponse<bool> response = await _authDAO.CreateTwoFactorCode (username);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success ("Usuario no encontrado.", false);
            return MessageResponse<bool>.Success ("Código doble factor eliminado.", true);
        }
    }
}
