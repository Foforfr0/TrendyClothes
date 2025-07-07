using AuctionAuctioneerService.DAO;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Consult;
using AuctionAuctioneerService.Services.Intefaces;

namespace AuctionAuctioneerService.Services.Implements {
    public class ConsultAuctionService : IConsultAuctionService {
        private readonly ConsultAuctionDAO _auctionDAO;

        public ConsultAuctionService (ConsultAuctionDAO auctionDAO) {
            _auctionDAO = auctionDAO;
        }

        public async Task<MessageResponse<List<MyAuctionsDTO>>> GetAuctionsByUserAsync (string username) {
            MessageResponse<List<Entities.AuctionsProduct>> response = await _auctionDAO.GetAuctionsUserAsync (username);

            if (response.IsError)
                return MessageResponse<List<MyAuctionsDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<MyAuctionsDTO>>.Success (response.Message, default);
            if (response.DataRetrieved.Count <= 0)
                return MessageResponse<List<MyAuctionsDTO>>.Success ("No se ha registrado una subasta todavía.", default);

            List<MyAuctionsDTO> auctions = response.DataRetrieved
                .Select (prod => new MyAuctionsDTO {
                    Id = prod.Id,
                    Name = prod.Name,
                    FirstPrice = prod.FirstPrice ?? 0,
                    DateStart = prod.DateStart,
                    DateEnd = prod.DateEnd,
                    BidsCount = prod.BidsAuctions?.Count ?? 0, // Protegido
                    LastPrice = prod.LastPrice ?? 0,
                    Status = prod.Status?.Status ?? "Desconocido", // Protegido
                    ImageBase64 = prod.PhotosAuctions?.FirstOrDefault ()?.Photo != null
                        ? Convert.ToBase64String (prod.PhotosAuctions.First ().Photo)
                        : string.Empty,
                    MimeImage = prod.PhotosAuctions?.FirstOrDefault ()?.Mime ?? string.Empty
                }).ToList ();
            return MessageResponse<List<MyAuctionsDTO>>.Success (response.Message, auctions);
        }

        public async Task<MessageResponse<AuctionDetailsDTO>> GetAuctionAsync (int id) {
            MessageResponse<Entities.AuctionsProduct> response = await _auctionDAO.GetAuctionAsync (id);

            if (response.IsError)
                return MessageResponse<AuctionDetailsDTO>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<AuctionDetailsDTO>.Success (response.Message, default);

            AuctionDetailsDTO auction = new AuctionDetailsDTO {
                Id = response.DataRetrieved.Id,
                Name = response.DataRetrieved.Name,
                FirstPrice = response.DataRetrieved.FirstPrice ?? 0,
                Bid = response.DataRetrieved.Bid,
                BidsCount = response.DataRetrieved.BidsAuctions.Count,
                LastPrice = response.DataRetrieved.LastPrice ?? 0,
                DateStart = response.DataRetrieved.DateStart,
                DateEnd = response.DataRetrieved.DateEnd,
                StatusId = response.DataRetrieved.StatusId,
                Status = response.DataRetrieved.Status.Status,
                Description = response.DataRetrieved.Description,
                ImageBase64 = response.DataRetrieved.PhotosAuctions.FirstOrDefault ()?.Photo != null
                    ? Convert.ToBase64String (response.DataRetrieved.PhotosAuctions.FirstOrDefault ().Photo)
                    : string.Empty,
                MimeImage = response.DataRetrieved.PhotosAuctions.FirstOrDefault ()?.Mime ?? string.Empty
            };
            return MessageResponse<AuctionDetailsDTO>.Success (response.Message, auction);
        }
    }
}
