using AuctionParticipantService.Entities;
using AuctionParticipantService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionParticipantService.DAO
{
    public class AuctionService : IAuctionService
    {
        private readonly TrendyClothesDBContext _context;
        private readonly ILogger<AuctionService> _logger;

        public AuctionService(TrendyClothesDBContext context, ILogger<AuctionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<AuctionDTO>> GetActiveAuctionsAsync()
        {
            try
            {
                return await _context.AuctionsProducts
                    .Where(a => a.StatusId == 1)
                    .Select(a => new AuctionDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        FirstPrice = a.FirstPrice,
                        Bid = a.Bid,
                        LastPrice = a.LastPrice,
                        DateStart = a.DateStart,
                        DateEnd = a.DateEnd,
                        Description = a.Description,
                        SellerId = a.SellerId,
                        ProductId = a.ProductId,
                        StatusId = a.StatusId
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las subastas activas.");
                return new List<AuctionDTO>();
            }
        }

        public async Task<bool> UpdateLastPriceAsync(int auctionId, decimal newLastPrice)
        {
            try
            {
                var auction = await _context.AuctionsProducts.FindAsync(auctionId);

                if (auction == null)
                {
                    _logger.LogWarning("No se encontró la subasta con ID: {AuctionId}", auctionId);
                    return false;
                }

                auction.LastPrice = newLastPrice;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el último precio de la subasta con ID: {auctionId}");
                return false;
            }
        }

        public async Task<AuctionDTO?> GetAuctionByIdAsync(int auctionId)
        {
            try
            {
                return await _context.AuctionsProducts
                    .Where(a => a.Id == auctionId)
                    .Select(a => new AuctionDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        FirstPrice = a.FirstPrice,
                        Bid = a.Bid,
                        LastPrice = a.LastPrice,
                        DateStart = a.DateStart,
                        DateEnd = a.DateEnd,
                        Description = a.Description,
                        SellerId = a.SellerId,
                        ProductId = a.ProductId,
                        StatusId = a.StatusId
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la subasta con ID {auctionId}");
                return null;
            }
        }

        Task<List<AuctionDTO>> IAuctionService.GetActiveAuctionsAsync()
        {
            throw new NotImplementedException();
        }

        Task<AuctionDTO?> IAuctionService.GetAuctionByIdAsync(int auctionId)
        {
            throw new NotImplementedException();
        }
    }
}
