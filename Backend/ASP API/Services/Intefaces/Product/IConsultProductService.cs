using Backend.DTO;
using Backend.DTO.Product.Consult;
using Backend.DTO.Product.MyProducts;

namespace Backend.Services.Intefaces.Product {
    public interface IConsultProductService {
        public Task<MessageResponse<List<ProductsDTO>>> GetProductsAsync (string? query);
        public Task<MessageResponse<List<MyProductsDTO>>> GetMyProductsAsync (string username);
        public Task<MessageResponse<ProductDetailsDTO>> GetDetailsAsync (int id);
        public Task<MessageResponse<MyProductDetailsDTO>> GetMyProductDetailsAsync (int id);
    }
}
