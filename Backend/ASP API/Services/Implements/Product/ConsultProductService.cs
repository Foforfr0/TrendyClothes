using Backend.DAO.Product;
using Backend.DTO;
using Backend.DTO.Product.Consult;
using Backend.DTO.Product.MyProducts;
using Backend.Services.Intefaces.Product;

namespace Backend.Services.Implements.Product {
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
