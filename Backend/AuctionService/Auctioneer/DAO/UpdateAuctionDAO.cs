using AuctionAuctioneerService.Entities;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Update;
using Microsoft.EntityFrameworkCore;

namespace AuctionAuctioneerService.DAO {
    public class UpdateAuctionDAO {
        private readonly TrendyClothesDBContext _context;

        public UpdateAuctionDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<bool>> UpdateAuctionAsync (UpdateLastPriceDTO updateAuctionDTO) {
            try {
                Entities.AuctionsProduct? currentAuction = await _context.AuctionsProducts
                   .Where (auction => auction.Id == updateAuctionDTO.Id)
                   .FirstOrDefaultAsync ();

                if (currentAuction == null)
                    return MessageResponse<bool>.Success ("Subasta no encontrada.", false);

                currentAuction.LastPrice = updateAuctionDTO.LastPrice;

                bool saveFailed = false;
                do {
                    try {
                        _context.Entry (currentAuction).State = EntityState.Modified;
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
                return MessageResponse<bool>.Success ("Último precio de subasta actualizado.", true);
            } catch (InvalidOperationException ex) {
                return MessageResponse<bool>.Failure ($"Error al actualizar último precio de subasta: {ex.Message}");
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }


        public async Task<MessageResponse<bool>> UpdateStatusAuctionAsync (UpdateStatusDTO updateAuctionDTO) {
            try {
                Entities.AuctionsProduct? currentAuction = await _context.AuctionsProducts
                   .Where (auction => auction.Id == updateAuctionDTO.Id)
                   .FirstOrDefaultAsync ();

                if (currentAuction == null)
                    return MessageResponse<bool>.Success ("Subasta no encontrada.", false);

                currentAuction.StatusId = updateAuctionDTO.StatusId ?? 1;

                bool saveFailed = false;
                do {
                    try {
                        _context.Entry (currentAuction).State = EntityState.Modified;
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
                return MessageResponse<bool>.Success ("Status de subasta actualizado.", true);
            } catch (InvalidOperationException ex) {
                return MessageResponse<bool>.Failure ($"Error al actualizar último precio de subasta: {ex.Message}");
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
