using Backend.DTO.Auction;

namespace Backend.Services.Intefaces.Auction
{
    public interface IAuctionService
    {
        Task<AuctionDTO?> GetAuctionAsync(int id);
        Task<bool> SubmitBidAsync(BidDTO bid);
    }
}
