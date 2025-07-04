using AuctionAuctioneerService.Models;

namespace AuctionAuctioneerService.Services.Intefaces {
    public interface IConsultImagesService {
        public Task<MessageResponse<byte[]>> GetImageAuctionId (int auctionId);
    }
}
