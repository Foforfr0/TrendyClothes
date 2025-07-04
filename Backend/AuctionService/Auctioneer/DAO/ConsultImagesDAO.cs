using AuctionAuctioneerService.Entities;
using AuctionAuctioneerService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAuctioneerService.DAO {
    public class ConsultImagesDAO {
        private readonly TrendyClothesDBContext _context;

        public ConsultImagesDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<byte[]>> GetImagesAsync (int auctionId) {
            try {
                byte[]? response = await _context.PhotosAuctions
                    .Where (pp => pp.AuctionId == auctionId)
                    .Select (pp => pp.Photo)
                    .FirstOrDefaultAsync ();

                return MessageResponse<byte[]>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<byte[]>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
