using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.User {
    public class AuthDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly UserDAO _userDAO;
        private readonly ManageEmail _manageEmail;

        public AuthDAO (TrendyClothesDBContext context, UserDAO userDAO, ManageEmail manageEmail) {
            _context = context;
            _userDAO = userDAO;
            this._manageEmail = manageEmail;
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

        public async Task<MessageResponse<jwtDTO>> ValidateTwoFactorCode (CodeTwoFactorDTO codeTwoFactorDTO) {
            try {
                // Validate Username
                if (await _userDAO.GetUserAsync (codeTwoFactorDTO.username) == null)
                    return MessageResponse<jwtDTO>.Success ("User not found.", null);

                // Validate if username has twoFactorCode
                if (string.IsNullOrEmpty (await _userDAO.GetTwoFactorCodeAsync (codeTwoFactorDTO.username)))
                    return MessageResponse<jwtDTO>.Success ("User doesn't have twoFactorCode.", null);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Entities.User? currentUser = await _context.Users.Where (user =>
                    user.Username.Equals (codeTwoFactorDTO.username) &&
                    user.TwoFactorCode.Equals (codeTwoFactorDTO.twoFactorCode))
                    .FirstOrDefaultAsync ();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                if (currentUser == null)
                    return MessageResponse<jwtDTO>.Success ("TwoFactorCode incorrect.", null);

                return MessageResponse<jwtDTO>.Success ("TwoFactorCode correct.",
                    new jwtDTO {
                        username = currentUser.Username ?? "---",
                        role = currentUser.TwoFactorCode ?? "---"
                    });
            } catch (Exception ex) {
                return MessageResponse<jwtDTO>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> CreateTwoFactorCode (EmailDTO emailDTO) {
            try {
                Entities.User? currentUser = await _context.Users
                   .Where (user => user.Username.Equals (emailDTO.username))
                   .FirstOrDefaultAsync ();

                if (currentUser == null)
                    return MessageResponse<bool>.Success ("Usuario no encontrado.", false);

                // TODO Just to try faster string twoFactorCode = new Random ().Next (100000, 999999).ToString ();
                currentUser.TwoFactorCode = "123456";

                bool saveFailed = false;
                do {
                    try {
                        _context.Entry (currentUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync ();
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

                // TODO Just to try faster await _manageEmail.SendAsync (emailDTO.username, emailDTO.email, twoFactorCode).ConfigureAwait (false);
                return MessageResponse<bool>.Success ("", true);
            } catch (InvalidOperationException ex) {
                return MessageResponse<bool>.Failure ($"Error al enviar el código doble factor al correo: {ex.Message}");
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> DeleteTwoFactorCode (string username) {
            try {
                Entities.User? currentUser = await _context.Users
                   .Where (user => user.Username.Equals (username))
                   .FirstOrDefaultAsync ();

                if (currentUser == null)
                    return MessageResponse<bool>.Success ("Usuario no encontrado.", false);

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
