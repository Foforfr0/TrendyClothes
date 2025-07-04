using Microsoft.EntityFrameworkCore;
using ProductService.Entities;
using ProductService.Models;

namespace ProductService.DAO {
    public class ConsultImagesDAO {
        private readonly TrendyClothesDBContext _context;
        readonly ILogger<ConsultTagsDAO> _logger;

        public ConsultImagesDAO (TrendyClothesDBContext context, ILogger<ConsultTagsDAO> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<MessageResponse<byte[]>> GetImagesAsync (int productId) {
            try {
                byte[]? response = await _context.PhotosProducts
                    .Where (pp => pp.ProductId == productId)
                    .Select (pp => pp.Photo)
                    .FirstOrDefaultAsync ();

                return MessageResponse<byte[]>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<byte[]>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
