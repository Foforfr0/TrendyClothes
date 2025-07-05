using AuctionAuctioneerService.DAO;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Create;
using AuctionAuctioneerService.Services.Intefaces;

namespace AuctionAuctioneerService.Services.Implements {
    public class CreateAuctionService : ICreateAuctionService {
        private readonly CreateAuctionDAO _createAuctionDAO;

        public CreateAuctionService (CreateAuctionDAO createAuctionDAO) {
            _createAuctionDAO = createAuctionDAO;
        }

        public async Task<MessageResponse<bool>> CreateAuctionAsync (CreateAuctionDTO createAuctionDTO) {
            MessageResponse<bool> response = await _createAuctionDAO.PostAuctionAsync (createAuctionDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
