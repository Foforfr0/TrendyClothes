using Backend.DAO.Product;
using Backend.DTO;
using Backend.DTO.Product.Seller;
using Backend.Services.Intefaces.Product;

namespace Backend.Services.Implements.Product {
    public class EditProductService : IEditProductService {
        private readonly EditProductDAO _editProductDAO;

        public EditProductService (EditProductDAO editProductDAO) {
            _editProductDAO = editProductDAO;
        }

        public async Task<MessageResponse<bool>> PutProductAsync (EditProductDTO editProductDTO) {
            MessageResponse<bool> response = await _editProductDAO.PutProductAsync (editProductDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
