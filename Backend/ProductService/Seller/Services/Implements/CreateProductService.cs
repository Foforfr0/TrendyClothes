using ProductSellerService.DAO;
using ProductSellerService.Models;
using ProductSellerService.Services.Interfaces;

namespace ProductSellerService.Services.Implements {
    public class CreateProductService : ICreateProductService {
        private readonly CreateProductDAO _createProductDAO;

        public CreateProductService (CreateProductDAO createProductDAO) {
            _createProductDAO = createProductDAO;
        }

        public async Task<MessageResponse<int>> PostProductAsync (NewProductDTO newProduct) {
            MessageResponse<int> response = await _createProductDAO.PostProductAsync (newProduct);

            if (response.IsError)
                return MessageResponse<int>.Failure (response.Message);
            if (response.DataRetrieved <= 0)
                return MessageResponse<int>.Success (response.Message, 0);
            return MessageResponse<int>.Success (response.Message, response.DataRetrieved);
        }
    }
}
