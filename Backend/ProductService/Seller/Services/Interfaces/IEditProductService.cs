using ProductSellerService.Models;

namespace ProductSellerService.Services.Intefaces {
    public interface IEditProductService {
        public Task<MessageResponse<bool>> PutProductAsync (EditProductDTO editProductDTO);
    }
}
