using ProductSellerService.Models;

namespace ProductSellerService.Services.Intefaces {
    public interface IConsultProductService {
        public Task<MessageResponse<List<MyProductsDTO>>> GetMyProductsAsync (string username);
        public Task<MessageResponse<MyProductDetailsDTO>> GetMyProductDetailsAsync (int id);
    }
}
