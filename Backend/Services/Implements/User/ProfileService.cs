using Backend.DAO.User;
using Backend.DTO.User.Profile;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class ProfileService : IProfileService{
        private readonly ProfileDAO _profileDAO;

        public ProfileService (ProfileDAO profileDAO) {
            _profileDAO = profileDAO;            
        }

        public Task<ViewMyProfileDTO?> GetViewProfileAsync (int id) {
            return null;
        }
    }
}
