using Backend.DTO.User.Profile;

namespace Backend.Services.Intefaces.User {
    public interface IProfileService {
        public Task<ViewMyProfileDTO?> GetViewProfileAsync (int id);
    }
}
