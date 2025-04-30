using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.User.Auth {
    public class AuthDAO {
        private readonly TrendyClothesDBContext _context;

        public AuthDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<Entities.User?> PostLoginAsync (string Username, string Password) {
            return await _context.Users.Where (user =>
                    user.Username.Equals (Username) &&
                    user.Password.Equals (Password))
                .FirstOrDefaultAsync ();
        }
    }
}
