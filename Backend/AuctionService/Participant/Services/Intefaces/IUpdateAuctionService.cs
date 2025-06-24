using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Update;

namespace AuctionParticipantService.Services.Intefaces {
    public interface IUpdateAuctionService {
        public Task<MessageResponse<bool>> UpdateLastPriceAsync (UpdateLastPriceDTO updateAuctionDTO);
        public Task<MessageResponse<bool>> UpdateStatusAsync (UpdateStatusDTO updateAuctionDTO);
    }
}
