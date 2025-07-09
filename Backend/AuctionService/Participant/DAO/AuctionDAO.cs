using AuctionParticipantService.Entities;
using AuctionParticipantService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionParticipantService.DAO {
    public class AuctionDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly ILogger<AuctionDAO> _logger;

        public AuctionDAO (TrendyClothesDBContext context, ILogger<AuctionDAO> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<MessageResponse<List<AuctionDTO>>> GetActiveAuctionsWithPhotoAsync () {
            try {
                var results = await (from auction in _context.AuctionsProducts
                                     join photo in _context.PhotosAuctions
                                     on auction.Id equals photo.AuctionId
                                     where auction.StatusId == 1
                                     select new AuctionDTO {
                                         Id = auction.Id,
                                         Name = auction.Name,
                                         FirstPrice = auction.FirstPrice,
                                         Bid = auction.Bid,
                                         LastPrice = auction.LastPrice,
                                         DateStart = auction.DateStart,
                                         DateEnd = auction.DateEnd,
                                         SellerId = auction.SellerId,
                                         StatusId = auction.StatusId,
                                         Description = auction.Description,
                                         Photo = photo.Photo,
                                         Mime = photo.Mime
                                     })
                                     .ToListAsync ();

                return new MessageResponse<List<AuctionDTO>> (true, "Subastas activas con foto recuperadas", results);
            } catch (Exception ex) {
                _logger.LogError (ex, "Error al recuperar subastas activas con foto.");
                return new MessageResponse<List<AuctionDTO>> (false, "Error al recuperar datos", null);
            }
        }

        public async Task<MessageResponse<AuctionDTO>> GetAuctionByIdAsync (int id) {
            try {
                var auction = await (from a in _context.AuctionsProducts
                                     join p in _context.PhotosAuctions on a.Id equals p.AuctionId
                                     where a.Id == id
                                     select new AuctionDTO {
                                         Id = a.Id,
                                         Name = a.Name,
                                         FirstPrice = a.FirstPrice,
                                         Bid = a.Bid,
                                         LastPrice = a.LastPrice,
                                         DateStart = a.DateStart,
                                         DateEnd = a.DateEnd,
                                         SellerId = a.SellerId,
                                         StatusId = a.StatusId,
                                         Description = a.Description,
                                         Photo = p.Photo,
                                         Mime = p.Mime
                                     }).FirstOrDefaultAsync ();

                if (auction == null)
                    return new MessageResponse<AuctionDTO> (false, "No se encontró la subasta", null);

                return new MessageResponse<AuctionDTO> (true, "Subasta recuperada con éxito", auction);
            } catch (Exception ex) {
                _logger.LogError (ex, "Error al recuperar subasta por ID.");
                return new MessageResponse<AuctionDTO> (false, "Error del servidor", null);
            }
        }

        public async Task<MessageResponse<bool>> IncreaseLastPriceAsync (int auctionId) {
            try {
                var auction = await _context.AuctionsProducts.FindAsync (auctionId);
                if (auction == null)
                    return new MessageResponse<bool> (false, "Subasta no encontrada", false);

                auction.LastPrice += auction.Bid;
                await _context.SaveChangesAsync ();

                return new MessageResponse<bool> (true, "Puja realizada con éxito", true);
            } catch (Exception ex) {
                _logger.LogError (ex, "Error al incrementar el precio de la subasta.");
                return new MessageResponse<bool> (false, "Error al actualizar el precio", false);
            }
        }
        public async Task<MessageResponse<bool>> RegisterBidAsync (BidDTO bid) {
            try {
                var newBid = new BidsAuction {
                    AuctionId = bid.AuctionId,
                    BuyerId = bid.BuyerId
                };

                await _context.BidsAuctions.AddAsync (newBid);
                await _context.SaveChangesAsync ();

                return new MessageResponse<bool> (true, "Puja registrada correctamente", true);
            } catch (Exception ex) {
                _logger.LogError (ex, "Error al registrar la puja.");
                return new MessageResponse<bool> (false, "Error al registrar la puja", false);
            }
        }

        public async Task<MessageResponse<bool>> UpdateExpiredAuctionsAsync () {
            try {
                var now = DateTime.UtcNow;

                var expiredAuctions = await _context.AuctionsProducts
                    .Where (a => a.DateEnd < now)
                    .ToListAsync ();

                if (expiredAuctions.Count == 0)
                    return MessageResponse<bool>.Success ("No hay subastas vencidas.", false);

                foreach (var auction in expiredAuctions) {
                    auction.StatusId = 2;
                }

                await _context.SaveChangesAsync ();
                return MessageResponse<bool>.Success ("Subastas vencidas actualizadas.", true);
            } catch (Exception ex) {
                _logger.LogError (ex, "Error actualizando subastas vencidas");
                return MessageResponse<bool>.Failure ("Error actualizando subastas vencidas.");
            }
        }

        public async Task<MessageResponse<int>> GetBuyerIdByUsernameAsync(string username)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Username == username)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();

                if (user == 0)
                    return new MessageResponse<int>(false, "No se encontró un usuario con ese username", 0);

                return new MessageResponse<int>(true, "ID del usuario recuperado", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ID del usuario por username.");
                return new MessageResponse<int>(false, "Error al buscar usuario", 0);
            }
        }

        public async Task<MessageResponse<List<AuctionDTO>>> GetWonAuctionsByBuyerAsync(int buyerId)
        {
            try
            {
                var wonAuctions = await (
                    from auction in _context.AuctionsProducts
                    where auction.StatusId == 4
                    let lastBid = (
                        from bid in _context.BidsAuctions
                        where bid.AuctionId == auction.Id
                        orderby bid.Id descending
                        select bid
                    ).FirstOrDefault()
                    where lastBid != null && lastBid.BuyerId == buyerId
                    join photo in _context.PhotosAuctions
                        on auction.Id equals photo.AuctionId into photoJoin
                    from photo in photoJoin.DefaultIfEmpty()
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
                        StatusId = auction.StatusId,
                        Description = auction.Description,
                        Photo = photo.Photo,
                        Mime = photo.Mime
                    }).ToListAsync();

                return new MessageResponse<List<AuctionDTO>>(true, "Subastas ganadas recuperadas", wonAuctions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar subastas ganadas.");
                return new MessageResponse<List<AuctionDTO>>(false, "Error al recuperar datos", null);
            }
        }



    }
}
