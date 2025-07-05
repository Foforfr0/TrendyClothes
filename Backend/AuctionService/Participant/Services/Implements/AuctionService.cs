using AuctionParticipantService.Models;
using AuctionParticipantService.Services.Intefaces;
using AuctionParticipantService.DAO;

namespace AuctionParticipantService.Services.Implements
{
    public class AuctionService : IAuctionService
    {
        private readonly AuctionDAO _auctionDAO;

        public AuctionService(AuctionDAO auctionDAO)
        {
            _auctionDAO = auctionDAO;
        }

        public async Task<MessageResponse<List<AuctionDTO>>> GetActiveAuctionsWithPhotoAsync()
        {
            MessageResponse<List<AuctionDTO>> response = await _auctionDAO.GetActiveAuctionsWithPhotoAsync();

            if (response.IsError)
                return MessageResponse<List<AuctionDTO>>.Failure(response.Message);

            if (response.DataRetrieved == null || response.DataRetrieved.Count == 0)
                return MessageResponse<List<AuctionDTO>>.Success("No hay subastas activas con fotos.", default);

            return MessageResponse<List<AuctionDTO>>.Success(response.Message, response.DataRetrieved);
        }

    }
}
