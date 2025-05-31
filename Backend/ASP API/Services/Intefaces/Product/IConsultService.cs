using Backend.DTO;
using Backend.DTO.Product.Consult;

namespace Backend.Services.Intefaces.Product {
    public interface IConsultService {
        public Task<MessageResponse<List<SearchProducts>>> GetProductsAsync (string? query);
        public Task<MessageResponse<ViewDetailsDTO>> ViewDetailsAsync (int id);
    }
}
