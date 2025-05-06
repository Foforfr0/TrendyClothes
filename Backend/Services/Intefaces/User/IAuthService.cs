using Backend.DTO;
using Backend.DTO.User.Auth;

namespace Backend.Services.Intefaces.User {
    public interface IAuthService {
        public Task<MessageResponse<LoginDTO>> PostLoginAsync (LoginDTO loginDTO);
        public Task<MessageResponse<EmailDTO>> GetValidateEmailUserAsync (EmailDTO emailDTO);
        public Task<MessageResponse<bool>> PostTwoFactorCodeAsync (string username);
        public Task<MessageResponse<bool>> GetValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO);
        public Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username);
    }
}
