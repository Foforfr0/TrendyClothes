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

        public async Task<MessageResponse<AuctionDTO>> GetAuctionByIdAsync(int id)
        {
            MessageResponse<AuctionDTO> response = await _auctionDAO.GetAuctionByIdAsync(id);

            if (response.IsError || response.DataRetrieved == null)
                return MessageResponse<AuctionDTO>.Failure(response.Message);

            return MessageResponse<AuctionDTO>.Success(response.Message, response.DataRetrieved);
        }

        public async Task<MessageResponse<bool>> IncreaseLastPriceAsync(int auctionId)
        {
            MessageResponse<bool> response = await _auctionDAO.IncreaseLastPriceAsync(auctionId);

            if (response.IsError)
                return MessageResponse<bool>.Failure(response.Message);

            return MessageResponse<bool>.Success(response.Message, true);
        }

        public async Task<MessageResponse<bool>> RegisterBidAsync(BidDTO bid)
        {
            var result = await _auctionDAO.RegisterBidAsync(bid);

            if (result.IsError)
                return MessageResponse<bool>.Failure(result.Message);

            return MessageResponse<bool>.Success(result.Message, result.DataRetrieved);
        }

        public async Task<MessageResponse<bool>> UpdateExpiredAuctionsAsync()
        {
            var result = await _auctionDAO.UpdateExpiredAuctionsAsync();

            if (result.IsError)
                return MessageResponse<bool>.Failure(result.Message);

            return MessageResponse<bool>.Success(result.Message, result.DataRetrieved);
        }

        public async Task<MessageResponse<List<AuctionDTO>>> GetWonAuctionsByUsernameAsync(string username)
        {
            var userIdResult = await _auctionDAO.GetBuyerIdByUsernameAsync(username);

            if (userIdResult.IsError)
                return MessageResponse<List<AuctionDTO>>.Failure(userIdResult.Message ?? "Error al obtener ID del usuario");

            var response = await _auctionDAO.GetWonAuctionsByBuyerAsync(userIdResult.DataRetrieved);

            if (response.IsError)
                return MessageResponse<List<AuctionDTO>>.Failure(response.Message);

            if (response.DataRetrieved == null || response.DataRetrieved.Count == 0)
                return MessageResponse<List<AuctionDTO>>.Success("No hay subastas ganadas por este usuario.", default);

            return MessageResponse<List<AuctionDTO>>.Success(response.Message, response.DataRetrieved);
        }

        public async Task<MessageResponse<bool>> UpdateAuctionStatusToPaidAsync(int auctionId)
        {
            var response = await _auctionDAO.UpdateAuctionStatusToPaidAsync(auctionId);

            if (response.IsError)
                return MessageResponse<bool>.Failure(response.Message);

            if (!response.DataRetrieved)
                return MessageResponse<bool>.Success("No se realizó el cambio de estado.", false);

            return MessageResponse<bool>.Success(response.Message, true);
        }
    }
}
