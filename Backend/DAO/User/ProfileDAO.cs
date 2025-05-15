using Backend.DTO;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.User {
    public class ProfileDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly UserDAO _userDAO;

        public ProfileDAO (TrendyClothesDBContext context, UserDAO userDAO) {
            _context = context;
            _userDAO = userDAO;
        }

        public async Task<MessageResponse<Entities.User>> GetMyPersonalInformationUser (string username) {
            try {
                Entities.User? currentUser = await _userDAO.GetUserAsync (username);
                if (currentUser == null)
                    return MessageResponse<Entities.User>.Success ("Usuario no encontrado.", default);
                return MessageResponse<Entities.User>.Success ("", currentUser);
            } catch (Exception ex) {
                return MessageResponse<Entities.User>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.Address>>> GetAddressesUser (String username) {
            try {
                Entities.User? currentUser = await _userDAO.GetUserAsync (username);
                if (currentUser == null)
                    return MessageResponse<List<Entities.Address>>.Success ("Usuario no encontrado.", default);

                List<Entities.Address> addresses = await _context.Users
                    .Where (u => u.Username == username)
                    .SelectMany (u => u.User_Addresses!) // accede a la colección intermedia
                    .Select (ua => ua.Address!)
                    .ToListAsync ();
                return MessageResponse<List<Entities.Address>>.Success ("Direcciones recuperadas.", null);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.Address>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
