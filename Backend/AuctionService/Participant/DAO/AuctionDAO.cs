using AuctionParticipantService.Entities;
using AuctionParticipantService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionParticipantService.DAO
{
    public class AuctionDAO
    {
        private readonly TrendyClothesDBContext _context;
        private readonly ILogger<AuctionDAO> _logger;

        public AuctionDAO(TrendyClothesDBContext context, ILogger<AuctionDAO> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MessageResponse<List<AuctionDTO>>> GetActiveAuctionsWithPhotoAsync()
        {
            try
            {
                var results = await (from auction in _context.AuctionsProducts
                                     join photo in _context.PhotosAuctions
                                     on auction.Id equals photo.AuctionId
                                     where auction.StatusId == 1
                                     select new AuctionDTO
                                     {
                                         Id = auction.Id,
                                         Name = auction.Name,
                                         FirstPrice = auction.FirstPrice,
                                         Bid = auction.Bid,
                                         LastPrice = auction.LastPrice,
                                         DateStart = auction.DateStart,
                                         DateEnd = auction.DateEnd,
                                         SellerId = auction.SellerId,
                                         ProductId = auction.ProductId,
                                         StatusId = auction.StatusId,
                                         Description = auction.Description,
                                         Photo = photo.Photo,
                                         Mime = photo.Mime
                                     })
                                     .ToListAsync();

                return new MessageResponse<List<AuctionDTO>>(true, "Subastas activas con foto recuperadas", results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar subastas activas con foto.");
                return new MessageResponse<List<AuctionDTO>>(false, "Error al recuperar datos", null);
            }
        }
    }
}
