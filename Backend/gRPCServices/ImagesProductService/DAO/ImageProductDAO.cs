using ImagesProductService.DTO;
using ImagesProductService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImagesProductService.DAO {
    public class ImageProductDAO {
        private readonly TrendyClothesDBContext _context;

        public ImageProductDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<OneImageDTO?> GetOneImage (int idProduct) {
            OneImageDTO? image = await _context.PhotosProducts
                .Where (p => p.ProductId == idProduct)
                .Select (p => new OneImageDTO {
                    image = p.Photo,
                    mime = p.Mime
                })
                .FirstOrDefaultAsync ();

            return image;
        }
    }
}
