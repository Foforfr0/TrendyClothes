using AuctionParticipantService.DAO;
using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Consult;
using AuctionParticipantService.Services.Intefaces;

namespace AuctionParticipantService.Services.Implements
{
    public class ConsultAuctionService : IConsultAuctionService
    {
        private readonly ConsultAuctionDAO _auctionDAO;
        private readonly AuctionsDAO _auctionsDAO;

        public ConsultAuctionService(ConsultAuctionDAO auctionDAO, AuctionsDAO auctionsDAO)
        {
            _auctionDAO = auctionDAO;
            _auctionsDAO = auctionsDAO;
        }

        public async Task<MessageResponse<List<AuctionsDTO>>> GetAuctionsAsync(string? query)
        {
            MessageResponse<List<Entities.AuctionsProduct>> response;
            if (string.IsNullOrEmpty(query))
                response = await _auctionDAO.GetAuctionsAsync();
            else
                response = await _auctionDAO.GetAuctionsAsync(query);

            if (response.IsError)
                return MessageResponse<List<AuctionsDTO>>.Failure(response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<AuctionsDTO>>.Success(response.Message, default);
            if (response.DataRetrieved.Count <= 0)
                return MessageResponse<List<AuctionsDTO>>.Success("Ninguna subasta correspende con la consulta deseada.", default);

            List<AuctionsDTO> auctions = response.DataRetrieved
                .Select(auct => new AuctionsDTO
                {
                    Id = auct.Id,
                    Name = auct.Name,
                    StartingPrice = auct.FirstPrice ?? 0,
                    StartDate = auct.DateStart,
                    EndDate = auct.DateEnd,
                    SellerUsername = auct.Seller.Username,
                    BidsCount = auct.BidsAuctions.Count,
                    CurrentPrice = auct.LastPrice ?? 0
                }).ToList();
            return MessageResponse<List<AuctionsDTO>>.Success(response.Message, auctions);
        }

        public async Task<MessageResponse<List<AuctionsDTO>>> GetAuctionsByUserAsync(string username)
        {
            MessageResponse<List<Entities.AuctionsProduct>> response = await _auctionDAO.GetAuctionsUserAsync(username);

            if (response.IsError)
                return MessageResponse<List<AuctionsDTO>>.Failure(response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<AuctionsDTO>>.Success(response.Message, default);
            if (response.DataRetrieved.Count <= 0)
                return MessageResponse<List<AuctionsDTO>>.Success("No se ha registrado una subasta todavía.", default);

            List<AuctionsDTO> auctions = response.DataRetrieved
                .Select(prod => new AuctionsDTO
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    StartingPrice = prod.FirstPrice ?? 0,
                    StartDate = prod.DateStart,
                    EndDate = prod.DateEnd,
                    SellerUsername = prod.Seller.Username,
                    BidsCount = prod.BidsAuctions.Count,
                    CurrentPrice = prod.LastPrice ?? 0
                }).ToList();
            return MessageResponse<List<AuctionsDTO>>.Success(response.Message, auctions);
        }

        public async Task<MessageResponse<AuctionDetailsDTO>> GetAuctionAsync(int id)
        {
            MessageResponse<Entities.AuctionsProduct> response = await _auctionDAO.GetAuctionAsync(id);

            if (response.IsError)
                return MessageResponse<AuctionDetailsDTO>.Failure(response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<AuctionDetailsDTO>.Success(response.Message, default);

            AuctionDetailsDTO auction = new AuctionDetailsDTO
            {
                Id = response.DataRetrieved.Id,
                Name = response.DataRetrieved.Name,
                StartingPrice = response.DataRetrieved.FirstPrice ?? 0,
                StartDate = response.DataRetrieved.DateStart,
                EndDate = response.DataRetrieved.DateEnd,
                SellerUsername = response.DataRetrieved.Seller.Username,
                BidsCount = response.DataRetrieved.BidsAuctions.Count,
                CurrentPrice = response.DataRetrieved.LastPrice ?? 0
            };
            return MessageResponse<AuctionDetailsDTO>.Success(response.Message, auction);
        }

        public async Task<MessageResponse<List<AuctionFullDTO>>> GetActiveAuctionsAsync()
        {
            return await _auctionsDAO.GetActiveAuctionsAsync();
        }
    }
}
