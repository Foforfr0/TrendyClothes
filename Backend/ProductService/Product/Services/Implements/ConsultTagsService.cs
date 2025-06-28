using ProductService.DAO;
using ProductService.Models;
using ProductService.Services.Intefaces;

namespace ProductService.Services.Implements {
    public class ConsultTagsService : IConsultTagsService {
        private readonly ConsultTagsDAO _consultTagsDAO;
        private readonly ILogger<ConsultTagsService> _logger;

        public ConsultTagsService (ConsultTagsDAO consultTagsDAO, ILogger<ConsultTagsService> logger) {
            _consultTagsDAO = consultTagsDAO;
            _logger = logger;
        }

        public async Task<MessageResponse<List<CategoriesDTO>>> GetCategories () {
            MessageResponse<List<Entities.CategoriesProduct>> response = await _consultTagsDAO.GetCategoriesAsync ();
            if (response.IsError)
                return MessageResponse<List<CategoriesDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<CategoriesDTO>>.Success ("Lista de categorias de producto no obtenidas.", default);

            List<CategoriesDTO> listCategories = new List<CategoriesDTO> ();
            foreach (Entities.CategoriesProduct item in response.DataRetrieved) {
                CategoriesDTO category = new CategoriesDTO {
                    Id = item.Id,
                    Category = item.Category
                };
                listCategories.Add (category);
            }
            return MessageResponse<List<CategoriesDTO>>.Success (response.Message, listCategories);
        }

        public async Task<MessageResponse<List<TypesDTO>>> GetTypes () {
            MessageResponse<List<Entities.TypesProduct>> response = await _consultTagsDAO.GetTypesAsync ();
            if (response.IsError)
                return MessageResponse<List<TypesDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<TypesDTO>>.Success ("Lista de tipos de producto no obtenidos.", default);

            List<TypesDTO> listTypes = new List<TypesDTO> ();
            foreach (Entities.TypesProduct item in response.DataRetrieved) {
                TypesDTO type = new TypesDTO {
                    Id = item.Id,
                    Type = item.Type
                };

                listTypes.Add (type);
            }
            return MessageResponse<List<TypesDTO>>.Success (response.Message, listTypes);
        }

        public async Task<MessageResponse<List<StatussesDTO>>> GetStatusses () {
            MessageResponse<List<Entities.StatusesProduct>> response = await _consultTagsDAO.GetStatussesAsync ();
            if (response.IsError)
                return MessageResponse<List<StatussesDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<StatussesDTO>>.Success ("Lista de tipos de producto no obtenidos.", default);

            List<StatussesDTO> listTypes = new List<StatussesDTO> ();
            foreach (Entities.StatusesProduct item in response.DataRetrieved) {
                StatussesDTO statuse = new StatussesDTO {
                    Id = item.Id,
                    Status = item.Status
                };

                listTypes.Add (statuse);
            }
            return MessageResponse<List<StatussesDTO>>.Success (response.Message, listTypes);
        }
    }
}
