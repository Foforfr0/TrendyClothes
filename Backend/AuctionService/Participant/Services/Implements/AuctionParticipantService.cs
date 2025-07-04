using AuctionParticipantService.DAO;
using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Consult;
using AuctionParticipantService.Services.Interfaces;

namespace AuctionParticipantService.Services.Implementations
{
    public class AuctionParticipantService : IAuctionParticipantService
    {
        private readonly AuctionsDAO _dao;

        public AuctionParticipantService(AuctionsDAO dao)
        {
            _dao = dao;
        }

        public async Task<MessageResponse<List<AuctionFullDTO>>> GetActiveAuctionsAsync()
        {
            return await _dao.GetActiveAuctionsFullAsync();
        }
    }
}
