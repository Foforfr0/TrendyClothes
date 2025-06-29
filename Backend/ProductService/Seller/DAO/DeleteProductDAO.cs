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

                bool SaveFailed = false;
                do
                {
                    try
                    {
                        _context.Entry(productToDelete).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        SaveFailed = true;
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is Product)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                if (databaseValues != null)
                                {
                                    entry.OriginalValues.SetValues(databaseValues);
                                    entry.CurrentValues.SetValues(proposedValues);
                                }
                            }
                        }
                    }
                } while (SaveFailed);

                return MessageResponse<bool>.Success("Producto eliminado correctamente (eliminación lógica).", true);
            }
            catch (Exception ex)
            {
                return MessageResponse<bool>.Failure($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
