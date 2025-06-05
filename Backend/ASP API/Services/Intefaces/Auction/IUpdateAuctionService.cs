using Backend.DTO;
using Backend.DTO.Auction.Update;

namespace Backend.Services.Intefaces.Auction {
    public interface IUpdateAuctionService {
        public Task<MessageResponse<bool>> UpdateLastPriceAsync (UpdateLastPriceDTO updateAuctionDTO);
        public Task<MessageResponse<bool>> UpdateStatusAsync (UpdateStatusDTO updateAuctionDTO);
    }
}
