using Microsoft.EntityFrameworkCore;
using ProductSellerService.Entities;
using ProductSellerService.Models;

namespace ProductSellerService.DAO {
    public class EditProductDAO {
        private readonly TrendyClothesDBContext _context;

        public EditProductDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<bool>> PutProductAsync (EditProductDTO editProductDTO) {
            try {
                Entities.Product? currentProduct = await _context.Products.FindAsync (editProductDTO.Id);

                currentProduct.Name = editProductDTO.Name;
                currentProduct.Price = editProductDTO.Price;
                currentProduct.Discount = editProductDTO.Discount;
                currentProduct.StockAvailable = editProductDTO.StockAvailable;
                currentProduct.Description = editProductDTO.Description;
                currentProduct.CategoryId = editProductDTO.CategoryId;
                currentProduct.TypeId = editProductDTO.TypeId;
                currentProduct.StatusId = editProductDTO.StatusId;

                await _context.SaveChangesAsync ();


                bool SaveFailed = false;
                do {
                    try {
                        _context.Entry (currentProduct).State = EntityState.Modified;
                        _context.SaveChanges ();

                    } catch (DbUpdateConcurrencyException ex) {
                        SaveFailed = true;
                        foreach (var entry in ex.Entries) {
                            if (entry.Entity is Entities.Product) {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues ();

                                if (databaseValues != null) {
                                    var databaseEntity = (Entities.Product)databaseValues.ToObject ();
                                    // Actualiza los valores originales con los valores actuales de la base de datos.
                                    entry.OriginalValues.SetValues (databaseValues);
                                    // Decide qué hacer con los valores propuestos.
                                    entry.CurrentValues.SetValues (proposedValues);
                                }
                            }
                        }
                    }
                } while (SaveFailed);

                return MessageResponse<bool>.Success ($"Datos de producto actualizados.", true);
        } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
