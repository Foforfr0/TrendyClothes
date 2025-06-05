using Backend.DTO;
using Backend.DTO.Auction.Consult;

namespace Backend.Services.Intefaces.Auction {
    public interface IConsultAuctionService {
        public Task<MessageResponse<List<AuctionsDTO>>> GetAuctionsAsync (string? query);
        public Task<MessageResponse<List<AuctionsDTO>>> GetAuctionsByUserAsync (string username);
        public Task<MessageResponse<AuctionDetailsDTO>> GetAuctionAsync (int id);
    }
}
