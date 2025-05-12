using Backend.DAO.User;
using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class AuthService : IAuthService {
        private readonly ILogger<AuthService> _logger;
        private readonly AuthDAO _authDAO;

        public AuthService (ILogger<AuthService> logger, AuthDAO authDAO) {
            _logger = logger;
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

        public async Task<MessageResponse<bool>> PostTwoFactorCodeAsync (EmailDTO emailDTO) {
            MessageResponse<bool> response = await _authDAO.CreateTwoFactorCode (emailDTO);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success ("Usuario no encontrado.", false);
            return MessageResponse<bool>.Success ("Código doble factor enviado.", true);
        }

        public async Task<MessageResponse<CodeTwoFactorDTO>> GetValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO) {
            MessageResponse<CodeTwoFactorDTO> response = await _authDAO.ValidateTwoFactorCode (codeTwoFactorDTO);

            if (response.isError)
                return MessageResponse<CodeTwoFactorDTO>.Failure (response.message);
            if (response.dataRetrieved == null && response.message.Equals ("User not found"))
                return MessageResponse<CodeTwoFactorDTO>.Success ("Usuario no encontrado.", null);
            if (response.dataRetrieved == null && response.message.Equals ("User doesn't have twoFactorCode."))
                return MessageResponse<CodeTwoFactorDTO>.Success ("Usuario no posee un código doble factor.", null);
            if (response.dataRetrieved == null && response.message.Equals ("TwoFactorCode incorrect."))
                return MessageResponse<CodeTwoFactorDTO>.Success ("Código doble factor incorrecto.", null);

            _logger.LogInformation (response.dataRetrieved == null ? "response null" : "response no null");
            _logger.LogInformation (response.dataRetrieved.username == null ? "username null" : "username no null");
            _logger.LogInformation (response.dataRetrieved.username == "" ? "username empty" : "username no empty");
            _logger.LogInformation (response.dataRetrieved.role == null ? "role null" : "role no null");
            _logger.LogInformation (response.dataRetrieved.role == "" ? "role empty" : "role no empty");
            codeTwoFactorDTO.role = response.message;

            return MessageResponse<CodeTwoFactorDTO>.Success ("Código doble factor correcto.", codeTwoFactorDTO);
        }

        public async Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username) {
            MessageResponse<bool> response = await _authDAO.DeleteTwoFactorCode (username);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success ("Usuario no encontrado.", false);
            return MessageResponse<bool>.Success ("Código doble factor eliminado.", true);
        }
    }
}
