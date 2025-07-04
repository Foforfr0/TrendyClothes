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

        public async Task<MessageResponse<List<AuctionsListDTO>>> GetActiveAuctionsAsync()
        {
            try
            {
                var auctions = await _context.AuctionsProducts
                    .Include(a => a.Product)
                    .Include(a => a.Status)
                    .Where(a => a.StatusId == 1)
                    .ToListAsync();

                var result = new List<AuctionsListDTO>();
                _logger.LogInformation($"Subastas encontradas: {auctions.Count}");

                foreach (var auction in auctions)
                {
                    var photo = await _context.PhotosProducts
                        .Where(p => p.ProductId == auction.ProductId)
                        .Select(p => new { p.Photo, p.Mime })
                        .FirstOrDefaultAsync();

                    string base64Image = "";
                    string mime = "image/jpeg";

                    if (photo != null)
                    {
                        base64Image = Convert.ToBase64String(photo.Photo);
                        mime = photo.Mime ?? mime;
                    }

                    result.Add(new AuctionsListDTO
                    {
                        Id = auction.Id,
                        ProductName = auction.Name,
                        LastPrice = auction.LastPrice ?? auction.FirstPrice ?? 0,
                        ImageBase64 = base64Image,
                        MimeImage = mime
                    });
                }

                return MessageResponse<List<AuctionsListDTO>>.Success("Subastas activas obtenidas correctamente.", result);
            }
            catch (Exception ex)
            {
                return MessageResponse<List<AuctionsListDTO>>.Failure($"Error al consultar subastas activas: {ex.Message}");
            }
        }
    }
}
