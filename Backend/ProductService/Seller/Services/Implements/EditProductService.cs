using ProductSellerService.DAO;
using ProductSellerService.Models;
using ProductSellerService.Services.Intefaces;

namespace ProductSellerService.Services.Implements {
    public class EditProductService : IEditProductService {
        private readonly EditProductDAO _editProductDAO;
        private readonly ConsultProductDAO _consultProductDAO;

        public EditProductService (EditProductDAO editProductDAO, ConsultProductDAO consultProductDAO) {
            _editProductDAO = editProductDAO;
            _consultProductDAO = consultProductDAO;
        }

        public async Task<MessageResponse<bool>> PutProductAsync (EditProductDTO editProductDTO) {
            MessageResponse<Entities.Product> currentProduct = await _consultProductDAO.GetDetailsAsync (editProductDTO.Id);
            if (currentProduct.IsError)
                return MessageResponse<bool>.Failure (currentProduct.Message);
            if (currentProduct.DataRetrieved == null)
                return MessageResponse<bool>.Success ("No se encontró el producto.", false);

            MessageResponse<bool> response = await _editProductDAO.PutProductAsync (editProductDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
