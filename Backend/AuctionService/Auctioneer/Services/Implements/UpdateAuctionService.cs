using AuctionAuctioneerService.DAO;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Update;
using AuctionAuctioneerService.Services.Intefaces;

namespace AuctionAuctioneerService.Services.Implements {
    public class UpdateAuctionService : IUpdateAuctionService {
        private readonly UpdateAuctionDAO _updateAuctionDAO;

        public UpdateAuctionService (UpdateAuctionDAO updateAuctionDAO) {
            _updateAuctionDAO = updateAuctionDAO;
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
