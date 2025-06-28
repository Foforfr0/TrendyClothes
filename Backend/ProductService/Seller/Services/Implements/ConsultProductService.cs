
using ProductSellerService.DAO;
using ProductSellerService.Models;
using ProductSellerService.Services.Intefaces;

namespace ProductSellerService.Services.Implements {
    public class ConsultProductService : IConsultProductService {
        private readonly ConsultProductDAO _productDAO;

        public ConsultProductService (ConsultProductDAO productDAO) {
            _productDAO = productDAO;
        }

        public async Task<MessageResponse<List<MyProductsDTO>>> GetMyProductsAsync (string username) {
            MessageResponse<List<Entities.Product>> response = await _productDAO.GetProductsUserAsync (username);

            if (response.IsError)
                return MessageResponse<List<MyProductsDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<MyProductsDTO>>.Success (response.Message, default);
            if (response.DataRetrieved.Count <= 0)
                return MessageResponse<List<MyProductsDTO>>.Success ("El usuario no ha publicado algún producto.", default);

            List<MyProductsDTO> products = response.DataRetrieved
                .Select (prod => new MyProductsDTO {
                    Id = prod.Id,
                    Name = prod.Name,
                    Price = prod.Price,
                    Discount = prod.Discount ?? 0,
                    NumberSold = prod.NumberSold,
                    AverageStars = prod.AverageStars ?? 0,
                    StockAvailable = prod.StockAvailable,
                    Status = prod.Status.Status
                }).ToList ();
            return MessageResponse<List<MyProductsDTO>>.Success (response.Message, products);
        }

        public async Task<MessageResponse<MyProductDetailsDTO>> GetMyProductDetailsAsync (int id) {
            MessageResponse<Entities.Product> response = await _productDAO.GetDetailsAsync (id);
            if (response.IsError)
                return MessageResponse<MyProductDetailsDTO>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<MyProductDetailsDTO>.Success ("Datos no obtenidos del producto.", default);
            MyProductDetailsDTO product = new MyProductDetailsDTO {
                Id = response.DataRetrieved.Id,
                Name = response.DataRetrieved.Name,
                Price = response.DataRetrieved.Price,
                Discount = response.DataRetrieved.Discount ?? 0,
                NumberSold = response.DataRetrieved.NumberSold,
                AverageStars = response.DataRetrieved.AverageStars ?? 0,
                Description = response.DataRetrieved.Description ?? "Sin descripción.",
                StockAvailable = response.DataRetrieved.StockAvailable,
                CategoryId = response.DataRetrieved.CategoryId,
                Category = response.DataRetrieved.Category.Category,
                TypeId = response.DataRetrieved.TypeId,
                Type = response.DataRetrieved.Type.Type,
                StatusId = response.DataRetrieved.StatusId,
                Status = response.DataRetrieved.Status.Status
            };
            return MessageResponse<MyProductDetailsDTO>.Success (response.Message, product);
        }
    }
}
