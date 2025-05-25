using Backend.DTO;
using Backend.DTO.User.Auth;

namespace Backend.Services.Intefaces.User {
    public interface IAuthService {
        public Task<MessageResponse<LoginDTO>> ValidateLoginAsync (LoginDTO loginDTO);
        public Task<MessageResponse<EmailDTO>> ValidateEmailUserAsync (EmailDTO emailDTO);
        public Task<MessageResponse<bool>> PostTwoFactorCodeAsync (EmailDTO emailDTO);
        public Task<MessageResponse<jwtDTO>> ValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO);
        public Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username);
    }
}
