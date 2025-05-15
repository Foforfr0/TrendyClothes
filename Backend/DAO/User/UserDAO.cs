using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.User {
    public class UserDAO {
        private readonly TrendyClothesDBContext _context;

        public UserDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<Entities.User?> GetUserAsync (string username) {
            try {
                Entities.User? response = await _context.Users
                    .Where (user => user.Username.Equals (username))
                    .FirstOrDefaultAsync ();

                return response;
            } catch (Exception ex) {
                throw new Exception ("Error al obtener el usuario.", ex);
            }
        }

        public async Task<string> GetTwoFactorCodeAsync (string username) {
            try {
                string? twoFactorCode = await _context.Users.Where (user =>
                    user.Username.Equals (username))
                    .Select (code => code.TwoFactorCode)
                    .FirstOrDefaultAsync ();

                return string.IsNullOrEmpty (twoFactorCode) ? "" : twoFactorCode;
            } catch (Exception ex) {
                throw new Exception ("Error al recuperar el código doble factor del usuario.", ex);
            }
        }
    }
}
