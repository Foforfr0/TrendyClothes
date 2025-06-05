using Backend.DTO;
using Backend.DTO.Product.Seller;

namespace Backend.Services.Intefaces.Product {
    public interface IEditProductService {
        public Task<MessageResponse<bool>> PutProductAsync (EditProductDTO editProductDTO);
    }
}
