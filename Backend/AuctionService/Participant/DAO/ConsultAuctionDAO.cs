using AuctionParticipantService.Entities;
using AuctionParticipantService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionParticipantService.DAO {
    public class ConsultAuctionDAO {
        private readonly TrendyClothesDBContext _context;

        public ConsultAuctionDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<List<Entities.AuctionsProduct>>> GetAuctionsAsync () {
            try {
                List<Entities.AuctionsProduct> response = await _context.AuctionsProducts
                    .Include (p => p.Seller)
                    .Include (p => p.BidsAuctions)
                    .ToListAsync ();

                return MessageResponse<List<Entities.AuctionsProduct>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.AuctionsProduct>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.AuctionsProduct>>> GetAuctionsAsync (string query) {
            try {
                List<Entities.AuctionsProduct> response = await _context.AuctionsProducts
                    .Include (p => p.Seller)
                    .Include (p => p.BidsAuctions)
                    .Where (p =>
                        query.Contains (p.Name) ||
                        query.Contains (p.FirstPrice.ToString () ?? "0") ||
                        query.Contains (p.Seller.Username))
                    .ToListAsync ();

                return MessageResponse<List<Entities.AuctionsProduct>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.AuctionsProduct>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.AuctionsProduct>>> GetAuctionsUserAsync (string username) {
            try {
                List<Entities.AuctionsProduct> response = await _context.AuctionsProducts
                    .Include (p => p.Seller)
                    .Include (p => p.BidsAuctions)
                    .Where (p => p.Seller.Username.Equals (username))
                    .ToListAsync ();

                return MessageResponse<List<Entities.AuctionsProduct>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.AuctionsProduct>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<Entities.AuctionsProduct>> GetAuctionAsync (int id) {
            try {
                Entities.AuctionsProduct? response = await _context.AuctionsProducts
                    .Include (p => p.Seller)
                    .Include (p => p.BidsAuctions)
                    .Where (p => p.Id == id)
                    .FirstOrDefaultAsync ();

                return MessageResponse<Entities.AuctionsProduct>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<Entities.AuctionsProduct>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.AuctionsProduct>>> GetAuctionsParticipated (string username) {
            try {
                List<Entities.AuctionsProduct> response = await _context.BidsAuctions
                    .Include (b => b.Auction)
                    .Include (b => b.Buyer)
                    .Where (p => p.Buyer.Username.Equals (username))
                    .Select (p => p.Auction)
                    .ToListAsync ();

                return MessageResponse<List<Entities.AuctionsProduct>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.AuctionsProduct>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}