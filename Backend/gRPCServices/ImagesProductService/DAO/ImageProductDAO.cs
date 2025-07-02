using ImagesProductService.DTO;
using ImagesProductService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImagesProductService.DAO {
    public class ImageProductDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly ILogger<ImageProductDAO> _logger;

        public ImageProductDAO (TrendyClothesDBContext context, ILogger<ImageProductDAO> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<OneImageDTO?> GetOneImage (int idProduct) {
            return await _context.PhotosProducts
                .Where (p => p.ProductId == idProduct)
                .Select (p => new OneImageDTO {
                    image = p.Photo,
                    mime = p.Mime
                })
                .FirstOrDefaultAsync ();
        }

        public async Task<bool> SaveOneImage (int idProduct, byte[] photo) {
            try {
                var existingImage = await _context.PhotosProducts
                    .FirstOrDefaultAsync (p => p.ProductId == idProduct);

                if (existingImage != null) {
                    existingImage.Photo = photo;
                    await _context.SaveChangesAsync ();
                    return true; // operación exitosa
                }

                _logger.LogWarning ("No se encontró imagen existente para el producto con ID {ProductId}", idProduct);
                return false;
            } catch (Exception ex) {
                _logger.LogError (ex, "Error al actualizar imagen para el producto con ID {ProductId}", idProduct);
                return false;
            }
        }

        public async Task<bool> SaveNewImage (int idProduct, byte[] photo, string mime) {
            try {
                var exists = await _context.PhotosProducts
                    .AnyAsync (p => p.ProductId == idProduct);

                if (exists) {
                    _logger.LogWarning ("Ya existe una imagen para el producto con ID {ProductId}", idProduct);
                    return false; // o puedes decidir sobrescribir si así lo deseas
                }

                var newImage = new PhotosProduct {
                    ProductId = idProduct,
                    Photo = photo,
                    Mime = mime
                };

                await _context.PhotosProducts.AddAsync (newImage);
                await _context.SaveChangesAsync ();
                return true;
            } catch (Exception ex) {
                _logger.LogError (ex, "Error al guardar nueva imagen para el producto con ID {ProductId}", idProduct);
                return false;
            }
        }
    }
}
