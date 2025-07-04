using AuctionParticipantService.Models;

namespace AuctionParticipantService.DAO
{
    public interface IAuctionService
    {
        Task<List<AuctionDTO>> GetActiveAuctionsAsync();
        Task<bool> UpdateLastPriceAsync(int auctionId, decimal newLastPrice);
        Task<AuctionDTO?> GetAuctionByIdAsync(int auctionId);
    }
}
