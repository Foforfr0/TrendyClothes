using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Update;

namespace AuctionAuctioneerService.Services.Intefaces {
    public interface IUpdateAuctionService {
        public Task<MessageResponse<bool>> UpdateStatusAsync (UpdateStatusDTO updateAuctionDTO);
    }
}
