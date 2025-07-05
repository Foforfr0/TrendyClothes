using AuctionParticipantService.Models;

namespace AuctionParticipantService.Services.Intefaces
{
    public interface IAuctionService
    {
        public Task<MessageResponse<List<AuctionDTO>>> GetActiveAuctionsWithPhotoAsync();
    }
}
