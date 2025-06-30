using ProductSellerService.DAO;
using ProductSellerService.Models;
using ProductSellerService.Services.Interfaces;

namespace ProductSellerService.Services.Implements
{
    public class CreateProductService : ICreateProductService
    {
        private readonly CreateProductDAO _createProductDAO;

        public CreateProductService(CreateProductDAO createProductDAO)
        {
            _createProductDAO = createProductDAO;
        }

        public async Task<MessageResponse<bool>> PostProductAsync(CreateProductDTO createProductDTO)
        {
            MessageResponse<bool> response = await _createProductDAO.PostProductAsync(createProductDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure(response.Message);

            return MessageResponse<bool>.Success(response.Message, true);
        }
    }
}
