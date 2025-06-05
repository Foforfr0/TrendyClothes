using Backend.DAO.Auction;
using Backend.DTO;
using Backend.DTO.Auction.Update;
using Backend.Services.Intefaces.Auction;

namespace Backend.Services.Implements.Auction {
    public class UpdateAuctionService : IUpdateAuctionService {
        private readonly UpdateAuctionDAO _updateAuctionDAO;

        public UpdateAuctionService (UpdateAuctionDAO updateAuctionDAO) {
            _updateAuctionDAO = updateAuctionDAO;
        }

        public async Task<MessageResponse<bool>> UpdateLastPriceAsync (UpdateLastPriceDTO updateAuctionDTO) {
            MessageResponse<bool> response = await _updateAuctionDAO.UpdateAuctionAsync (updateAuctionDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }

        public async Task<MessageResponse<bool>> UpdateStatusAsync (UpdateStatusDTO updateAuctionDTO) {
            MessageResponse<bool> response = await _updateAuctionDAO.UpdateStatusAuctionAsync (updateAuctionDTO);

            if (response.IsError)
                return MessageResponse<bool>.Failure (response.Message);
            if (response.DataRetrieved == false)
                return MessageResponse<bool>.Success (response.Message, false);
            return MessageResponse<bool>.Success (response.Message, true);
        }
    }
}
