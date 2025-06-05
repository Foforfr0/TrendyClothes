using Backend.DAO.User;
using Backend.DTO;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class DeleteAccountService : IDeleteAccountService {
        private readonly RegistrationDAO _registrationDAO;

        public DeleteAccountService (RegistrationDAO registrationDAO) {
            _registrationDAO = registrationDAO;
        }

        public async Task<MessageResponse<bool>> DeleteUserAsync (string username) {
            MessageResponse<bool> response = await _registrationDAO.DeleteUserAsync (username);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
