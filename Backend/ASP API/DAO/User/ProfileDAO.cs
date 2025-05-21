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

        public async Task<MessageResponse<Entities.User>> GetMyPersonalInformationUserAsync (string username) {
            try {
                Entities.User? currentUser = await _userDAO.GetUserAsync (username);
                if (currentUser == null)
                    return MessageResponse<Entities.User>.Success ("Usuario no encontrado.", default);
                return MessageResponse<Entities.User>.Success ("Información obtenida.", currentUser);
            } catch (Exception ex) {
                return MessageResponse<Entities.User>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.Address>>> GetAddressesUserAsync (String username) {
            try {
                Entities.User? currentUser = await _userDAO.GetUserAsync (username);
                if (currentUser == null)
                    return MessageResponse<List<Entities.Address>>.Success ("Usuario no encontrado.", default);

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                List<Entities.Address> addresses = await _context.Users
                    .Where (u => u.Username == username)
                    .SelectMany (u => u.User_Addresses) // accede a la colección intermedia
                    .Select (ua => ua.Address)
                    .ToListAsync ();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
                return MessageResponse<List<Entities.Address>>.Success ("Direcciones recuperadas.", addresses);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.Address>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
