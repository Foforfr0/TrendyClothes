using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.User {
    public class AuthDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly UserDAO _userDAO;

        public AuthDAO (TrendyClothesDBContext context, UserDAO userDAO) {
            _context = context;
            _userDAO = userDAO;
        }

        public async Task<MessageResponse<Entities.User>> ValidateLogin (LoginDTO loginDTO) {
            try {
                return MessageResponse<Entities.User>.Success ("",
                    await _context.Users.Where (user =>
                        user.Username.Equals (loginDTO.username) &&
                        user.Password.Equals (loginDTO.password))
                        .FirstOrDefaultAsync ());
            } catch (Exception ex) {
                return MessageResponse<Entities.User>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<Entities.User>> ValidateEmailUser (EmailDTO emailDTO) {
            try {
                return MessageResponse<Entities.User>.Success ("",
                    await _context.Users.Where (user =>
                        user.Username.Equals (emailDTO.username) &&
                        user.Email.Equals (emailDTO.email))
                        .FirstOrDefaultAsync ());
            } catch (Exception ex) {
                return MessageResponse<Entities.User>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> CreateTwoFactorCode (string username) {
            try {
                Entities.User? currentUser = await _context.Users
                   .Where (user => user.Username.Equals (username))
                   .FirstOrDefaultAsync ();

                if (currentUser == null)
                    return MessageResponse<bool>.Success ("Usuario no encontrado.", false);

                // TODO Send by email twoFactorCode
                currentUser.TwoFactorCode = new Random ().Next (100000, 999999).ToString (); // Código real

                bool saveFailed = false;
                do {
                    try {
                        _context.Entry (currentUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync ();
                        saveFailed = false;
                    } catch (DbUpdateConcurrencyException ex) {
                        saveFailed = true;
                        foreach (var entry in ex.Entries) {
                            if (entry.Entity is Entities.User) {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues ();

                                if (databaseValues != null) {
                                    entry.OriginalValues.SetValues (databaseValues);
                                    entry.CurrentValues.SetValues (proposedValues);
                                }
                            }
                        }
                    }
                } while (saveFailed);

                return MessageResponse<bool>.Success ("Código doble factor creado.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<Entities.User>> ValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO) {
            try {
                // Validate Username
                Entities.User? user = await _context.Users.Where (user =>
                    user.Username.Equals (codeTwoFactorDTO.username))
                    .FirstOrDefaultAsync ();
                if (user == null)
                    return MessageResponse<Entities.User>.Success ("User not found.", null);

                // Validate if username has twoFactorCode
                string? twoFactorCode = await _context.Users.Where (user =>
                    user.Username.Equals (codeTwoFactorDTO.username))
                    .Select (code => code.TwoFactorCode)
                    .FirstOrDefaultAsync ();
                if (string.IsNullOrEmpty (twoFactorCode))
                    return MessageResponse<Entities.User>.Success ("User doesn't have twoFactorCode.", null);

                Entities.User? currentUser = await _context.Users.Where (user =>
                    user.Username.Equals (codeTwoFactorDTO.username) &&
                    user.TwoFactorCode.Equals (codeTwoFactorDTO.twoFactorCode))
                    .FirstOrDefaultAsync ();

                if (currentUser == null)
                    return MessageResponse<Entities.User>.Success ("TwoFactorCode incorrect.", null);
                else
                    return MessageResponse<Entities.User>.Success ("TwoFactorCode correct.", currentUser);
            } catch (Exception ex) {
                return MessageResponse<Entities.User>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> DeleteTwoFactorCode (string username) {
            try {
                Entities.User? currentUser = await _context.Users
                   .Where (user => user.Username.Equals (username))
                   .FirstOrDefaultAsync ();

                if (currentUser == null)
                    return MessageResponse<bool>.Success ("Usuario no encontrado.", false);

                // TODO Send by email twoFactorCode
                currentUser.TwoFactorCode = "";

                bool saveFailed = false;
                do {
                    try {
                        _context.Entry (currentUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync ();
                        saveFailed = false;
                    } catch (DbUpdateConcurrencyException ex) {
                        saveFailed = true;
                        foreach (var entry in ex.Entries) {
                            if (entry.Entity is Entities.User) {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues ();

                                if (databaseValues != null) {
                                    entry.OriginalValues.SetValues (databaseValues);
                                    entry.CurrentValues.SetValues (proposedValues);
                                }
                            }
                        }
                    }
                } while (saveFailed);

                return MessageResponse<bool>.Success ("Código doble factor eliminado.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
