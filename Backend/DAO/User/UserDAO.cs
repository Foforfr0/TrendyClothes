using Backend.DTO;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.User {
    public class UserDAO {
        private readonly TrendyClothesDBContext _context;

        public UserDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<Entities.User>> GetUserAsync (string username) {
            try {
                Entities.User? response = await _context.Users
                    .Where (user => user.Username.Equals (username))
                    .FirstOrDefaultAsync ();

                return MessageResponse<Entities.User>.Success ("", response);
            } catch (Exception ex) {
                throw new Exception ("Error al obtener el usuario.", ex);
            }
        }
    }
}
