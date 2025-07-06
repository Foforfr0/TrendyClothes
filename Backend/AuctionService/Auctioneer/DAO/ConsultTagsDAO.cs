using AuctionAuctioneerService.Entities;
using AuctionAuctioneerService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAuctioneerService.DAO {
    public class ConsultTagsDAO {
        private readonly TrendyClothesDBContext _context;

        public ConsultTagsDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<List<StatusesAuction>>> GetCategoriesAsync () {
            try {
                List<StatusesAuction> categories = await _context.StatusesAuctions.ToListAsync ();

                return MessageResponse<List<StatusesAuction>>.Success ("", categories);
            } catch (Exception ex) {
                return MessageResponse<List<StatusesAuction>>.Failure ($"Error al obtener la lista de estados de subasta: {ex.Message}");
            }
        }
    }
}
