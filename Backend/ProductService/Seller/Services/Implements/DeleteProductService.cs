using ProductSellerService.DAO;
using ProductSellerService.Models;
using ProductSellerService.Services.Interfaces;

namespace ProductSellerService.Services.Implements
{
    public class DeleteProductService : IDeleteProductService
    {
        private readonly DeleteProductDAO _deleteProductDAO;

        public DeleteProductService(DeleteProductDAO deleteProductDAO)
        {
            _deleteProductDAO = deleteProductDAO;
        }

        public async Task<MessageResponse<bool>> DeleteProductAsync(int productId)
        {
            MessageResponse<bool> response = await _deleteProductDAO.DeleteProductAsync(productId);

            if (response.IsError)
                return MessageResponse<bool>.Failure(response.Message);

            return MessageResponse<bool>.Success(response.Message, true);
        }
    }
}
