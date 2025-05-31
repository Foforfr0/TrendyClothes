using Backend.DAO.Product;
using Backend.DTO;
using Backend.DTO.Product.Consult;
using Backend.Services.Intefaces.Product;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Backend.Services.Implements.Product {
    public class ConsultService : IConsultService {
        private readonly ConsultDAO _productDAO;

        public ConsultService (ConsultDAO productDAO) {
            _productDAO = productDAO;
        }

        public async Task<MessageResponse<List<SearchProducts>>> GetProductsAsync (string? query) {
            MessageResponse<List<Entities.Product>> response;
            if (string.IsNullOrEmpty (query))
                response = await _productDAO.GetProductsAsync ();
            else
                response = await _productDAO.GetProductsAsync (query);

            if (response.isError)
                return MessageResponse<List<SearchProducts>>.Failure (response.message);
            if (response.dataRetrieved == null)
                return MessageResponse<List<SearchProducts>>.Success (response.message, default);
            if (response.dataRetrieved.Count <= 0)
                return MessageResponse<List<SearchProducts>>.Success ("Ningún producto correspende con la consulta deseada.", default);

            List<SearchProducts> products = response.dataRetrieved
                .Select (prod => new SearchProducts {
                    id = prod.Id,
                    name = prod.Name,
                    price = prod.Price,
                    discount = prod.Discount,
                    numberSold = prod.NumberSold,
                    averageStars = prod.AverageStars,
                    stockAvailable = prod.StockAvailable,
                    category = prod.Category?.Category ?? "Sin categoría",
                    type = prod.Type?.Type ?? "Sin tipo",
                }).ToList ();
            return MessageResponse<List<SearchProducts>>.Success (response.message, products);
        }

        public async Task<MessageResponse<ViewDetailsDTO>> ViewDetailsAsync (int id) {
            MessageResponse<Entities.Product> response = await _productDAO.GetDetailsAsync (id);

            if (response.isError)
                return MessageResponse<ViewDetailsDTO>.Failure (response.message);
            if (response.dataRetrieved == null)
                return MessageResponse<ViewDetailsDTO>.Success ("Datos no obtenidos del producto.", default);

            ViewDetailsDTO product = new ViewDetailsDTO {
                id = response.dataRetrieved.Id,
                name = response.dataRetrieved.Name,
                price = response.dataRetrieved.Price,
                discount = response.dataRetrieved.Discount,
                numberSold = response.dataRetrieved.NumberSold,
                averageStars = response.dataRetrieved.AverageStars,
                description = response.dataRetrieved.Description,
                stockAvailable = response.dataRetrieved.StockAvailable,
                sellerId = response.dataRetrieved.SellerId,
                sellerName = response.dataRetrieved.Seller.FirstName+" "+response.dataRetrieved.Seller.MiddleName+" "+ response.dataRetrieved.Seller.LastName,
                category = response.dataRetrieved.Category.Category
            };

            return MessageResponse<ViewDetailsDTO>.Success (response.message, product);
        }
    }
}
