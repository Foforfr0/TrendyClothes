using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Consult;

namespace AuctionAuctioneerService.Services.Intefaces {
    public interface IConsultAuctionService {
        public Task<MessageResponse<List<AuctionsDTO>>> GetAuctionsByUserAsync (string username);
        public Task<MessageResponse<AuctionDetailsDTO>> GetAuctionAsync (int id);
    }
}
