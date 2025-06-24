using AuthService.Models;

namespace AuthService.Services.Intefaces {
    public interface IAuthService {
        public Task<MessageResponse<LoginDTO>> ValidateLoginAsync (LoginDTO loginDTO);
        public Task<MessageResponse<EmailDTO>> ValidateEmailUserAsync (EmailDTO emailDTO);
        public Task<MessageResponse<bool>> PostTwoFactorCodeAsync (EmailDTO emailDTO);
        public Task<MessageResponse<jwtDTO>> ValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO);
        public Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username);
    }
}
