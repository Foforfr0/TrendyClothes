using ProductSellerService.Models;

namespace ProductSellerService.Services.Interfaces
{
    public interface IDeleteProductService
    {
        public Task<MessageResponse<bool>> DeleteProductAsync(int productId);
    }
}
