using AuctionParticipantService.Entities;
using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Consult;
using Microsoft.EntityFrameworkCore;

namespace AuctionParticipantService.DAO
{
    public class AuctionsDAO
    {
        private readonly TrendyClothesDBContext _context;
        private ILogger<AuctionsDAO> _logger;

        public AuctionsDAO(TrendyClothesDBContext context, ILogger<AuctionsDAO> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MessageResponse<List<AuctionFullDTO>>> GetActiveAuctionsFullAsync()
        {
            try
            {
                var activeStatus = await _context.StatusesAuctions
                    .Where(s => s.Status == "Activo")
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();

                var auctions = await _context.AuctionsProducts
                    .Include(a => a.Status)
                    .Include(a => a.Product)
                    .Include(a => a.Seller)
                    .Where(a => a.StatusId == activeStatus)
                    .ToListAsync();

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
                    SellerId = a.SellerId,
                    StatusName = a.Status.Status,
                    SellerName = $"{a.Seller.FirstName} {a.Seller.LastName}"
                }).ToList();

                return MessageResponse<List<AuctionFullDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener subastas activas completas.");
                return MessageResponse<List<AuctionFullDTO>>.Error("Error al obtener subastas activas.");
            }
        }

    }
}
