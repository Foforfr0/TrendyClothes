using Backend.DAO.User;
using Backend.DTO;
using Backend.DTO.User.Registration;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class RegistrationService : IRegistrationService {
        private readonly RegistrationDAO _registrationDAO;

        public RegistrationService (RegistrationDAO registrationDAO) {
            _registrationDAO = registrationDAO;
        }

        public async Task<MessageResponse<bool>> PostUserAsync (RegistrationUserDTO newUserDTO) {
            MessageResponse<bool> response = await _registrationDAO.PostUserAsync (newUserDTO);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success (response.message, false);
            return MessageResponse<bool>.Success (response.message, true);
        }

        public async Task<MessageResponse<bool>> DeleteUserAsync (string username) {
            MessageResponse<bool> response = await _registrationDAO.DeleteUserAsync (username);

            if (response.isError)
                return MessageResponse<bool>.Failure (response.message);
            if (response.dataRetrieved == false)
                return MessageResponse<bool>.Success (response.message, false);
            return MessageResponse<bool>.Success (response.message, true);
        }
    }
}
