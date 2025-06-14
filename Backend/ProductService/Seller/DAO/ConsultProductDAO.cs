using Microsoft.EntityFrameworkCore;
using ProductSellerService.Entities;
using ProductSellerService.Models;

namespace ProductSellerService.DAO {
    public class ConsultProductDAO {
        private readonly TrendyClothesDBContext _context;

        public ConsultProductDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<List<Entities.Product>>> GetProductsAsync () {
            try {
                List<Entities.Product> response = await _context.Products
                    .Include (p => p.Category)
                    .Include (p => p.Type)
                    .Where (p => p.Status.Id == 1)
                    .ToListAsync ();

                return MessageResponse<List<Entities.Product>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.Product>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.Product>>> GetProductsAsync (string query) {
            try {
                List<Entities.Product> response = await _context.Products
                    .Include (p => p.Category)
                    .Include (p => p.Type)
                    .Where (p =>
                        query.Contains (p.Name) ||
                        query.Contains (p.Category.Category) ||
                        query.Contains (p.Type.Type) ||
                        query.Contains (p.Status.Status) &&
                        p.Status.Id == 1)
                    .ToListAsync ();

                return MessageResponse<List<Entities.Product>>.Success ("Productos obtenidos.", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.Product>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.Product>>> GetProductsUserAsync (string username) {
            try {
                List<Entities.Product> response = await _context.Products
                    .Include (p => p.Seller)
                    .Include (p => p.Category)
                    .Include (p => p.Type)
                    .Include (p => p.Status)
                    .Where (p => p.Seller.Username.Equals (username))
                    .ToListAsync ();

                return MessageResponse<List<Entities.Product>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.Product>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<Entities.Product>> GetDetailsAsync (int id) {
            try {
                Entities.Product? response = await _context.Products
                    .Include (p => p.Seller)
                    .Include (p => p.Category)
                    .Include (p => p.Type)
                    .Include (p => p.Status)
                    .Where (p => p.Id == id)
                    .FirstOrDefaultAsync ();

                return MessageResponse<Entities.Product>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<Entities.Product>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
