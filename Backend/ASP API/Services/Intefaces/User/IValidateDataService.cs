using Backend.DTO;

namespace Backend.Services.Intefaces.User {
    public interface IValidateDataService {
        public Task<MessageResponse<bool>> VerifyExistsUsername (string username);
        public Task<MessageResponse<bool>> VerifyExistsEmail (string email);
        public Task<MessageResponse<bool>> VerifyExistsPhoneNumber (string areaCode, string phoneNumber);
    }
}