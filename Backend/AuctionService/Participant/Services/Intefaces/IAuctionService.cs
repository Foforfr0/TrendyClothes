using AuctionParticipantService.Models;

namespace AuctionParticipantService.Services.Intefaces
{
    public interface IAuctionService
    {
        Task<MessageResponse<List<AuctionDTO>>> GetActiveAuctionsWithPhotoAsync();
        Task<MessageResponse<AuctionDTO>> GetAuctionByIdAsync(int id);
        Task<MessageResponse<bool>> IncreaseLastPriceAsync(int auctionId);
        Task<MessageResponse<bool>> RegisterBidAsync(BidDTO bid);
        Task<MessageResponse<bool>> UpdateExpiredAuctionsAsync();
        Task<MessageResponse<List<AuctionDTO>>> GetWonAuctionsByUsernameAsync(string username);

    }
}
