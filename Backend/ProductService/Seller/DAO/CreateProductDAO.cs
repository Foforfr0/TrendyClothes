using Microsoft.EntityFrameworkCore;
using ProductSellerService.Entities;
using ProductSellerService.Models;

namespace ProductSellerService.DAO
{
    public class CreateProductDAO
    {
        private readonly TrendyClothesDBContext _context;

        public CreateProductDAO(TrendyClothesDBContext context)
        {
            _context = context;
        }

        public async Task<MessageResponse<bool>> PostProductAsync(EditProductDTO createProductDTO)
        {
            try
            {
                Product newProduct = new Product
                {
                    Name = createProductDTO.Name,
                    Price = createProductDTO.Price,
                    Discount = createProductDTO.Discount,
                    StockAvailable = createProductDTO.StockAvailable,
                    Description = createProductDTO.Description,
                    CategoryId = createProductDTO.CategoryId,
                    TypeId = createProductDTO.TypeId,
                    StatusId = createProductDTO.StatusId
                };

                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                bool SaveFailed = false;
                do
                {
                    try
                    {
                        _context.Entry(newProduct).State = EntityState.Added;
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

                return MessageResponse<bool>.Success("Producto creado correctamente.", true);
            }
            catch (Exception ex)
            {
                return MessageResponse<bool>.Failure($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
