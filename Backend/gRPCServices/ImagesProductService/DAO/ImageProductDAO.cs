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

        public async Task<bool> SaveOneImage (int idProduct, byte[] photo) {
            try {
                PhotosProduct? image = await _context.PhotosProducts
                    .Where (image => image.ProductId == idProduct)
                    .FirstOrDefaultAsync ();

                if (image != null) {
                    image.Photo = photo;
                    await _context.SaveChangesAsync ();
                    return false;
                } else {
                    return true;
                }

            } catch {
                return true;
            }
        }

        public async Task<bool> SaveNewImage (int idProduct, byte[] photo) {
            try {
                PhotosProduct? pp = await _context.PhotosProducts
                    .Where (image => image.ProductId == idProduct)
                    .FirstOrDefaultAsync ();

                if (pp != null) {
                    pp.Photo = photo;
                    await _context.PhotosProducts.AddAsync (pp);
                    await _context.SaveChangesAsync ();
                    return false;
                } else {
                    return true;
                }

            } catch {
                return true;
            }
        }
    }
}
