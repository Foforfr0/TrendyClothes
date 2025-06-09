using Backend.DTO;
using Backend.DTO.Auction.Create;

namespace Backend.Services.Intefaces.Auction {
    public interface ICreateAuctionService {
        public Task<MessageResponse<bool>> CreateAuctionAsync (CreateAuctionDTO createAuctionDTO, string username);
    }
}
