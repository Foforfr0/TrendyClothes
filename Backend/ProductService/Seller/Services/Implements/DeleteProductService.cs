using ProductSellerService.DAO;
using ProductSellerService.Models;
using ProductSellerService.Services.Interfaces;

namespace ProductSellerService.Services.Implements {
    public class DeleteProductService : IDeleteProductService {
        private readonly DeleteProductDAO _deleteDAO;

        public DeleteProductService (DeleteProductDAO deleteProductDAO) {
            _deleteDAO = deleteProductDAO;
        }

        public async Task<MessageResponse<bool>> DeleteUserAsync (int id) {
            MessageResponse<bool> response = await _deleteDAO.DeleteProductAsync (id);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
