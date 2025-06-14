using ProductService.Models;

namespace ProductService.Services.Intefaces {
    public interface IConsultTagsService {
        public Task<MessageResponse<List<CategoriesDTO>>> GetCategories ();
        public Task<MessageResponse<List<TypesDTO>>> GetTypes ();
        public Task<MessageResponse<List<StatussesDTO>>> GetStatusses();
    }
}
