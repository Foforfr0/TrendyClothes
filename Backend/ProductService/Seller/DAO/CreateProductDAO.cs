using ProductSellerService.Entities;
using ProductSellerService.Models;

namespace ProductSellerService.DAO {
    public class CreateProductDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly ConsultUserDAO _consultUserDAO;

        public CreateProductDAO (TrendyClothesDBContext context, ConsultUserDAO consultUserDAO) {
            _context = context;
            _consultUserDAO = consultUserDAO;
        }
        public async Task<MessageResponse<int>> PostProductAsync (NewProductDTO newProduct) {
            try {
                int UserId = await _consultUserDAO.GetIdUserFromUsername (newProduct.UsernameSeller);

                Entities.Product product = new Entities.Product {
                    Name = newProduct.Name,
                    Price = newProduct.Price,
                    Discount = newProduct.Discount,
                    StockAvailable = newProduct.StockAvailable,
                    Description = newProduct.Description,
                    SellerId = UserId,
                    CategoryId = newProduct.CategoryId,
                    TypeId = newProduct.TypeId,
                    StatusId = newProduct.StatusId,
                };
                await _context.Products.AddAsync (product);
                await _context.SaveChangesAsync ();

                int newProductId = product.Id;

                return MessageResponse<int>.Success ("Producto creado exitosamente.", newProductId);
            } catch (Exception ex) {
                return MessageResponse<int>.Failure ($"Error al crear el producto: {ex.Message}");
            }
        }
    }
}
