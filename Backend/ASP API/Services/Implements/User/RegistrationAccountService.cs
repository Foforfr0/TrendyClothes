using Backend.DAO.User;
using Backend.DTO;
using Backend.DTO.User.Registration;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class RegistrationAccountService : IRegistrationAccountService {
        private readonly RegistrationDAO _registrationDAO;

        public RegistrationAccountService (RegistrationDAO registrationDAO) {
            _registrationDAO = registrationDAO;
        }

        public async Task<MessageResponse<bool>> PostUserAsync (RegistrationUserDTO newUserDTO) {
            MessageResponse<bool> response = await _registrationDAO.PostUserAsync (newUserDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
