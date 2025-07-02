using ProductSellerService.Models;

namespace ProductSellerService.Services.Interfaces {
    public interface IDeleteProductService {
        public Task<MessageResponse<bool>> DeleteUserAsync (int id);
    }
}
