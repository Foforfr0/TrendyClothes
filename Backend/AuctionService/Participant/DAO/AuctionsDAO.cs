using AuctionParticipantService.Entities;
using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Consult;
using Microsoft.EntityFrameworkCore;

namespace AuctionParticipantService.DAO
{
    public class AuctionsDAO
    {
        private readonly TrendyClothesDBContext _context;
        private readonly ILogger<AuctionsDAO> _logger;

        public AuctionsDAO(TrendyClothesDBContext context, ILogger<AuctionsDAO> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MessageResponse<List<AuctionFullDTO>>> GetActiveAuctionsFullAsync()
        {
            try
            {
                // Recuperar todas las subastas cuyo StatusId sea 1 (Activo)
                var auctions = await _context.AuctionsProducts
                    .Where(a => a.StatusId == 1)
                    .ToListAsync();

                if (auctions == null || !auctions.Any())
                {
                    return MessageResponse<List<AuctionFullDTO>>.Success("No hay subastas activas disponibles.", new());
                }

                var result = auctions.Select(a => new AuctionFullDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    FirstPrice = a.FirstPrice,
                    Bid = a.Bid,
                    LastPrice = a.LastPrice > 0 ? a.LastPrice : a.FirstPrice,
                    DateStart = a.DateStart,
                    DateEnd = a.DateEnd,
                    Description = a.Description,
                    ProductId = a.ProductId,
                    StatusId = a.StatusId,
                    SellerId = a.SellerId
                }).ToList();

                return MessageResponse<List<AuctionFullDTO>>.Success("Subastas activas recuperadas correctamente.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener subastas activas.");
                return MessageResponse<List<AuctionFullDTO>>.Error("Hubo un problema al obtener las subastas activas.");
            }
        }
    }
}
