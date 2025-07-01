using Microsoft.EntityFrameworkCore;
using ProductSellerService.Entities;
using ProductSellerService.Models;

namespace ProductSellerService.DAO
{
    public class DeleteProductDAO
    {
        private readonly TrendyClothesDBContext _context;

        public DeleteProductDAO(TrendyClothesDBContext context)
        {
            _context = context;
        }

        public async Task<MessageResponse<bool>> DeleteProductAsync(int productId)
        {
            try
            {
                Product? productToDelete = await _context.Products.FindAsync(productId);

                if (productToDelete == null)
                {
                    return MessageResponse<bool>.Failure("Producto no encontrado.");
                }

                productToDelete.StatusId = 2; // Eliminado
                await _context.SaveChangesAsync();

                return MessageResponse<bool>.Success("Producto eliminado correctamente (eliminación lógica).", true);
            }
            catch (Exception ex)
            {
                return MessageResponse<bool>.Failure($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
