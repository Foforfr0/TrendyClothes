using Backend.DTO;
using Backend.DTO.Product;

namespace Backend.Services.Intefaces.Product {
    public interface IConsultTagsService {
        public Task<MessageResponse<List<CategoriesDTO>>> GetCategories ();
        public Task<MessageResponse<List<TypesDTO>>> GetTypes ();
        public Task<MessageResponse<List<StatussesDTO>>> GetStatusses();
    }
}
