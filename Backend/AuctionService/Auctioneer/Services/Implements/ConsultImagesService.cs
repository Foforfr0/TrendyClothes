using AuctionAuctioneerService.DAO;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Services.Intefaces;

namespace AuctionAuctioneerService.Services.Implements {
    public class ConsultImagesService : IConsultImagesService {
        private readonly ConsultImagesDAO _consultImagesDAO;

        public ConsultImagesService (ConsultImagesDAO consultImagesDAO) {
            _consultImagesDAO = consultImagesDAO;
        }

        public async Task<MessageResponse<byte[]>> GetImageAuctionId (int auctionId) {
            MessageResponse<byte[]> response = await _consultImagesDAO.GetImagesAsync (auctionId);
            if (response.IsError)
                return MessageResponse<byte[]>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<byte[]>.Success ("Imagen de producto no obtenido.", default);

            return MessageResponse<byte[]>.Success (response.Message, response.DataRetrieved);
        }
    }
}
