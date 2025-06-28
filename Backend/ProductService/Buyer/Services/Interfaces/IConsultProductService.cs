using ProductBuyerService.Models;

namespace ProductBuyerService.Services.Intefaces {
    public interface IConsultProductService {
        public Task<MessageResponse<List<ProductsDTO>>> GetProductsAsync (string? query);
        public Task<MessageResponse<ProductDetailsDTO>> GetDetailsAsync (int id);
    }
}
