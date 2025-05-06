using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.User {
    public class ProfileDAO {
        private readonly TrendyClothesDBContext _context;

        public ProfileDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<Entities.User?> GetViewMyProfileAsync () {
            // TODO
            return await _context.Users.FirstOrDefaultAsync ();
        }
    }
}
