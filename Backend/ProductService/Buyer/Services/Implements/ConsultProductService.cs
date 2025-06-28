using ProductBuyerService.DAO;
using ProductBuyerService.Models;
using ProductBuyerService.Services.Intefaces;

namespace ProductBuyerService.Services.Implements {
    public class ConsultProductService : IConsultProductService {
        private readonly ConsultProductDAO _productDAO;

        public ConsultProductService (ConsultProductDAO productDAO) {
            _productDAO = productDAO;
        }

        public async Task<MessageResponse<List<ProductsDTO>>> GetProductsAsync (string? query) {
            MessageResponse<List<Entities.Product>> response;
            if (string.IsNullOrEmpty (query))
                response = await _productDAO.GetProductsAsync ();
            else
                response = await _productDAO.GetProductsAsync (query);

            if (response.IsError)
                return MessageResponse<List<ProductsDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<ProductsDTO>>.Success (response.Message, default);
            if (response.DataRetrieved.Count <= 0)
                return MessageResponse<List<ProductsDTO>>.Success ("Ningún producto correspende con la consulta deseada.", default);

            List<ProductsDTO> products = response.DataRetrieved
                .Select (prod => new ProductsDTO {
                    Id = prod.Id,
                    Name = prod.Name,
                    Price = prod.Price,
                    Discount = prod.Discount ?? 0,
                    NumberSold = prod.NumberSold,
                    AverageStars = prod.AverageStars ?? 0,
                    StockAvailable = prod.StockAvailable
                }).ToList ();
            return MessageResponse<List<ProductsDTO>>.Success (response.Message, products);
        }

        public async Task<MessageResponse<ProductDetailsDTO>> GetDetailsAsync (int id) {
            MessageResponse<Entities.Product> response = await _productDAO.GetDetailsAsync (id);

            if (response.IsError)
                return MessageResponse<ProductDetailsDTO>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<ProductDetailsDTO>.Success ("Datos no obtenidos del producto.", default);

            ProductDetailsDTO product = new ProductDetailsDTO {
                Id = response.DataRetrieved.Id,
                Name = response.DataRetrieved.Name,
                Price = response.DataRetrieved.Price,
                Discount = response.DataRetrieved.Discount ?? 0,
                NumberSold = response.DataRetrieved.NumberSold,
                AverageStars = response.DataRetrieved.AverageStars ?? 0,
                Description = response.DataRetrieved.Description ?? "Sin descripción.",
                StockAvailable = response.DataRetrieved.StockAvailable,
                SellerUsername = response.DataRetrieved.Seller.Username,
                Category = response.DataRetrieved.Category.Category,
                Type = response.DataRetrieved.Type.Type,
                StatusId = response.DataRetrieved.StatusId,
                Status = response.DataRetrieved.Status.Status
            };

            return MessageResponse<ProductDetailsDTO>.Success (response.Message, product);
        }
    }
}
