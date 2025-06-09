using Backend.DAO.Auction;
using Backend.DTO;
using Backend.DTO.Auction.Create;
using Backend.Services.Intefaces.Auction;

namespace Backend.Services.Implements.Auction {
    public class CreateAuctionService : ICreateAuctionService {
        private readonly CreateAuctionDAO _createAuctionDAO;

        public CreateAuctionService (CreateAuctionDAO createAuctionDAO) {
            _createAuctionDAO = createAuctionDAO;
        }

        public async Task<MessageResponse<bool>> CreateAuctionAsync (CreateAuctionDTO createAuctionDTO, string username) {
            MessageResponse<bool> response = await _createAuctionDAO.PostAuctionAsync (createAuctionDTO, username);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
