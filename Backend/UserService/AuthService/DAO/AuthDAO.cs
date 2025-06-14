using AuthService.Entities;
using AuthService.Helpers;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DAO {
    public class AuthDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly ConsultUserDAO _userDAO;
        private readonly ManageEmail _manageEmail; //TODO

        public AuthDAO (TrendyClothesDBContext context, ConsultUserDAO userDAO, ManageEmail manageEmail) {
            _context = context;
            _userDAO = userDAO;
            this._manageEmail = manageEmail;
        }

        public async Task<MessageResponse<Entities.User>> ValidateLoginAsync (LoginDTO loginDTO) {
            try {
                return MessageResponse<Entities.User>.Success ("",
                    await _context.Users.Where (user =>
                        user.Username.Equals (loginDTO.Username) &&
                        user.Password.Equals (loginDTO.Password))
                        .FirstOrDefaultAsync ());
            } catch (Exception ex) {
                return MessageResponse<Entities.User>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<Entities.User>> ValidateEmailUserAsync (EmailDTO emailDTO) {
            try {
                return MessageResponse<Entities.User>.Success ("",
                    await _context.Users.Where (user =>
                        user.Username.Equals (emailDTO.Username) &&
                        user.Email.Equals (emailDTO.Email))
                        .FirstOrDefaultAsync ());
            } catch (Exception ex) {
                return MessageResponse<Entities.User>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<jwtDTO>> ValidateTwoFactorCodeAsync (CodeTwoFactorDTO codeTwoFactorDTO) {
            try {
                // Validate Username
                if (await _userDAO.GetUserAsync (codeTwoFactorDTO.Username) == null)
                    return MessageResponse<jwtDTO>.Success ("Usuario no encontrado.", null);

                // Validate if Username has TwoFactorCode
                if (string.IsNullOrEmpty (await _userDAO.GetTwoFactorCodeAsync (codeTwoFactorDTO.Username)))
                    return MessageResponse<jwtDTO>.Success ("Usuario no posee un código doble factor.", null);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Entities.User? currentUser = await _context.Users
                    .Include (u => u.Role)
                    .Where (user =>
                    user.Username.Equals (codeTwoFactorDTO.Username) &&
                    user.TwoFactorCode.Equals (codeTwoFactorDTO.TwoFactorCode))
                    .FirstOrDefaultAsync ();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                if (currentUser == null)
                    return MessageResponse<jwtDTO>.Success ("Código doble factor incorrecto.", null);

                return MessageResponse<jwtDTO>.Success ("Código doble factor correcto.",
                    new jwtDTO {
                        Username = currentUser.Username ?? "---",
                        Role = currentUser.Role.Role ?? "---"
                    });
            } catch (Exception ex) {
                return MessageResponse<jwtDTO>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> PostTwoFactorCodeAsync (EmailDTO emailDTO) {
            try {
                Entities.User? currentUser = await _context.Users
                   .Where (user => user.Username.Equals (emailDTO.Username))
                   .FirstOrDefaultAsync ();

                if (currentUser == null)
                    return MessageResponse<bool>.Success ("Usuario no encontrado.", false);

                // TODO Just to try faster string TwoFactorCode = new Random ().Next (100000, 999999).ToString ();
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

                // TODO Just to try faster await _manageEmail.SendAsync (emailDTO.Username, emailDTO.Email, TwoFactorCode).ConfigureAwait (false);
                return MessageResponse<bool>.Success ("Código doble factor enviado.", true);
            } catch (InvalidOperationException ex) {
                return MessageResponse<bool>.Failure ($"Error al enviar el código doble factor al correo: {ex.Message}");
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> DeleteTwoFactorCodeAsync (string username) {
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
