using Microsoft.EntityFrameworkCore;
using AuctionAuctioneerService.Entities;

namespace AuctionAuctioneerService.DAO {
    public class ConsultUserDAO {
        private readonly TrendyClothesDBContext _context;

        public ConsultUserDAO (TrendyClothesDBContext context) {
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

        public async Task<int> GetIdUserFromUsername (string username) {
            try {
                int? userId = await _context.Users
                    .Where (user => user.Username.Equals (username))
                    .Select (user => user.Id)
                    .FirstOrDefaultAsync ();
                if (userId == null)
                    throw new Exception ("Usuario no encontrado.");
                return userId.Value;
            } catch (Exception ex) {
                throw new Exception ("Error al obtener el ID del usuario desde el token.", ex);
            }
        }

        public async Task<string?> GetUsernameAsync (string username) {
            try {
                string? response = await _context.Users
                    .Where (u => u.Username.Equals (username))
                    .Select (user => user.Username)
                    .FirstOrDefaultAsync ();

                return response;
            } catch (Exception ex) {
                throw new Exception ("Error al buscar el nombre de usuario.", ex);
            }
        }

        public async Task<string?> GetEmailAsync (string email) {
            try {
                string? response = await _context.Users
                    .Where (u => u.Email.Equals (email))
                    .Select (user => user.Email)
                    .FirstOrDefaultAsync ();

                return response;
            } catch (Exception ex) {
                throw new Exception ("Error al buscar el Email de usuario.", ex);
            }
        }

        public async Task<string?> GetAreaCodePhoneNumberAsync (string areaCode, string phoneNumber) {
            try {
                string? response = await _context.Users
                    .Where (u =>
                        u.AreaCode.Equals (areaCode) &&
                        u.PhoneNumber.Equals (phoneNumber))
                    .Select (user => user.AreaCode + user.PhoneNumber)
                    .FirstOrDefaultAsync ();

                return response;
            } catch (Exception ex) {
                throw new Exception ("Error al buscar el número de teléfono del usuario.", ex);
            }
        }

        public async Task<string?> GetTwoFactorCodeAsync (string username) {
            try {
                string? twoFactorCode = await _context.Users
                    .Where (user =>
                        user.Username.Equals (username))
                    .Select (code => code.TwoFactorCode)
                    .FirstOrDefaultAsync ();

                return twoFactorCode;
            } catch (Exception ex) {
                throw new Exception ("Error al recuperar el código doble factor del usuario.", ex);
            }
        }

        public async Task<string?> GetRoleUserAsync (string username) {
            try {
                string? role = await _context.Users
                    .Where (user =>
                        user.Username.Equals (username))
                    .Select (user => user.Role.Role)
                    .FirstOrDefaultAsync ();

                return role;
            } catch (Exception ex) {
                throw new Exception ("Error al recuperar el rol del usuario.", ex);
            }
        }
    }
}
