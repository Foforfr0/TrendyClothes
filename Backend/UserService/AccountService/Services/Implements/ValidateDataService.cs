using AccountService.DAO;
using AccountService.Models;
using AccountService.Services.Intefaces;

namespace AccountService.Services.Implements {
    public class ValidateDataService : IValidateDataService {
        private readonly ConsultUserDAO _userDAO;

        public ValidateDataService (ConsultUserDAO userDAO) {
            _userDAO = userDAO;
        }

        public async Task<MessageResponse<bool>> VerifyExistsUsername (string username) {
            try {
                string? response = await _userDAO.GetUsernameAsync (username);
                if (string.IsNullOrEmpty (response))
                    return MessageResponse<bool>.Success ("Nombre de usuario no existe.", false);
                return MessageResponse<bool>.Success ("Nombre de usuario si existe.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error al verififcar existencia del nombre del usuario: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> VerifyExistsEmail (string email) {
            try {
                string? response = await _userDAO.GetEmailAsync (email);
                if (string.IsNullOrEmpty (response))
                    return MessageResponse<bool>.Success ("Email de usuario no existe.", false);
                return MessageResponse<bool>.Success ("Email de usuario si existe.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error al verififcar existencia del Email de usuario: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> VerifyExistsPhoneNumber (string areaCode, string phoneNumber) {
            try {
                string? response = await _userDAO.GetAreaCodePhoneNumberAsync (areaCode, phoneNumber);
                if (string.IsNullOrEmpty (response))
                    return MessageResponse<bool>.Success ("Número de teléfono y lada no existe.", false);
                return MessageResponse<bool>.Success ("Número de teléfono y lada si existe.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error al verififcar existencia del número de teléfono de usuario: {ex.Message}");
            }
        }
    }
}
