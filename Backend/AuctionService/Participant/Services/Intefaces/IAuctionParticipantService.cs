using AuctionParticipantService.Models.Consult;
using AuctionParticipantService.Models;

namespace AuctionParticipantService.Services.Interfaces
{
    public interface IAuctionParticipantService
    {
        Task<MessageResponse<List<AuctionFullDTO>>> GetActiveAuctionsAsync();
    }
}
