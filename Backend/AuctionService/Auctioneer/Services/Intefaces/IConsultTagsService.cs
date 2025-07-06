using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Consult;

namespace AuctionAuctioneerService.Services.Intefaces {
    public interface IConsultTagsService {
        public Task<MessageResponse<List<StatusAuctionDTO>>> GetStatuses ();
    }
}
