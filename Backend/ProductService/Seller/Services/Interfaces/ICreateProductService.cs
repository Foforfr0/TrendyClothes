using ProductSellerService.Models;

namespace ProductSellerService.Services.Interfaces
{
    public interface ICreateProductService
    {
        public Task<MessageResponse<bool>> PostProductAsync(CreateProductDTO createProductDTO);
    }
}
