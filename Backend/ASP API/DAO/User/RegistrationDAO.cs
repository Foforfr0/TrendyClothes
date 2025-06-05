using Backend.DTO;
using Backend.DTO.User.Registration;
using Backend.Entities;

namespace Backend.DAO.User {
    public class RegistrationDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly ConsultUserDAO _userDAO;

        public RegistrationDAO (TrendyClothesDBContext context, ConsultUserDAO userDAO) {
            _context = context;
            _userDAO = userDAO;
        }

        public async Task<MessageResponse<bool>> PostUserAsync (RegistrationUserDTO newUserDTO) {
            try {
                if (newUserDTO == null)
                    return MessageResponse<bool>.Success ("Datos de usuario vacíos.", false);
                Entities.User newUser = new Entities.User ();
                newUser.FirstName = newUserDTO.FirstName;
                newUser.MiddleName = newUserDTO.MiddleName;
                newUser.LastName = newUserDTO.LastName;
                newUser.Username = newUserDTO.Username;
                newUser.Email = newUserDTO.Email;
                newUser.AreaCode = newUserDTO.AreaCode;
                newUser.PhoneNumber = newUserDTO.PhoneNumber;
                newUser.Password = newUserDTO.Password;
                newUser.RoleId = 2;

                _context.Users.Add (newUser);
                await _context.SaveChangesAsync ();

                return MessageResponse<bool>.Success ("Usuario registrado correctamente.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> DeleteUserAsync (string username) {
            try {
                Entities.User? currentUser = await _userDAO.GetUserAsync (username);
                if (currentUser == null)
                    return MessageResponse<bool>.Success ("Usuario no encontrado.", false);

                _context.Users.Remove (currentUser);
                await _context.SaveChangesAsync ();

                return MessageResponse<bool>.Success ("Usuario eliminado correctamente.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
