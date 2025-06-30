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

        public async Task<MessageResponse<bool>> PostProductAsync(CreateProductDTO createProductDTO)
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
                    ,SellerId = createProductDTO.SellerId
                };

                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                
                

                return MessageResponse<bool>.Success("Producto creado correctamente.", true);
            }
            catch (Exception ex)
            {
                return MessageResponse<bool>.Failure($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
