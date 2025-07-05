using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Create;

namespace AuctionAuctioneerService.Services.Intefaces {
    public interface ICreateAuctionService {
        public Task<MessageResponse<bool>> CreateAuctionAsync (CreateAuctionDTO createAuctionDTO);
    }
}
