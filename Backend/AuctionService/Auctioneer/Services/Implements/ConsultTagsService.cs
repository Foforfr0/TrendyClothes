using AuctionAuctioneerService.DAO;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Consult;
using AuctionAuctioneerService.Services.Intefaces;

namespace AuctionAuctioneerService.Services.Implements {
    public class ConsultTagsService : IConsultTagsService {
        private readonly ConsultTagsDAO _consultTagsDAO;

        public ConsultTagsService (ConsultTagsDAO consultTagsDAO) {
            _consultTagsDAO = consultTagsDAO;
        }

        public async Task<MessageResponse<List<StatusAuctionDTO>>> GetStatuses () {
            MessageResponse<List<Entities.StatusesAuction>> response = await _consultTagsDAO.GetCategoriesAsync ();
            if (response.IsError)
                return MessageResponse<List<StatusAuctionDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<StatusAuctionDTO>>.Success ("Lista de estados de subasta no obtenidos.", default);

            List<StatusAuctionDTO> listCategories = new List<StatusAuctionDTO> ();
            foreach (Entities.StatusesAuction item in response.DataRetrieved) {
                StatusAuctionDTO category = new StatusAuctionDTO {
                    Id = item.Id,
                    Status = item.Status
                };
                listCategories.Add (category);
            }
            return MessageResponse<List<StatusAuctionDTO>>.Success (response.Message, listCategories);
        }
    }
}
