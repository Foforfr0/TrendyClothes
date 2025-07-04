using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Consult;

namespace AuctionParticipantService.Services.Intefaces {
    public interface IConsultAuctionService {
        public Task<MessageResponse<List<AuctionsDTO>>> GetAuctionsAsync (string? query);
        public Task<MessageResponse<List<AuctionsDTO>>> GetAuctionsByUserAsync (string username);
        public Task<MessageResponse<AuctionDetailsDTO>> GetAuctionAsync (int id);
        Task<MessageResponse<List<AuctionsListDTO>>> GetActiveAuctionsAsync();

    }
}
