using ProductSellerService.Entities;
using ProductSellerService.Models;

namespace ProductSellerService.DAO {
    public class DeleteProductDAO {
        private readonly TrendyClothesDBContext _context;

        public DeleteProductDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<bool>> DeleteProductAsync (int id) {
            try {
                Product? product = await _context.Products.FindAsync (id);
                if (product == null) {
                    return MessageResponse<bool>.Success ("Producto no encontrado.", false);
                }
                _context.Products.Remove (product);
                await _context.SaveChangesAsync ();
                return MessageResponse<bool>.Success ("Producto eliminado correctamente.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error al eliminar el producto: {ex.Message}");
            }
        }
    }
}
