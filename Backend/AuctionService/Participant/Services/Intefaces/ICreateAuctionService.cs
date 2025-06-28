using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Create;

namespace AuctionParticipantService.Services.Intefaces {
    public interface ICreateAuctionService {
        public Task<MessageResponse<bool>> CreateAuctionAsync (CreateAuctionDTO createAuctionDTO, string username);
    }
}
