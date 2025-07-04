using AuctionAuctioneerService.Entities;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Create;
using Microsoft.EntityFrameworkCore;

namespace AuctionAuctioneerService.DAO {
    public class CreateAuctionDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly ConsultUserDAO _consultUserDAO;

        public CreateAuctionDAO (TrendyClothesDBContext context, ConsultUserDAO consultUserDAO) {
            _context = context;
            _consultUserDAO = consultUserDAO;
        }

        public async Task<MessageResponse<bool>> PostAuctionAsync (CreateAuctionDTO createAuctionDTO, string username) {
            try {
                int idUser = await _consultUserDAO.GetIdUserFromUsername (username);

                if (createAuctionDTO == null)
                    return MessageResponse<bool>.Success ("Datos de subasta vacíos.", false);
                Entities.AuctionsProduct newAuction = new Entities.AuctionsProduct ();
                newAuction.Name = createAuctionDTO.Name;
                newAuction.FirstPrice = createAuctionDTO.FirstPrice;
                newAuction.Bid = createAuctionDTO.MinBid;
                newAuction.LastPrice = createAuctionDTO.FirstPrice;
                newAuction.DateStart = createAuctionDTO.DateStart;
                newAuction.DateEnd = createAuctionDTO.DateEnd;
                newAuction.SellerId = idUser;
                newAuction.ProductId = createAuctionDTO.ProductId;
                newAuction.StatusId = 1;

                _context.AuctionsProducts.Add (newAuction);
                await _context.SaveChangesAsync ();

                return MessageResponse<bool>.Success ("Subasta registrada correctamente.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<bool>> PostBidAsync (CreateBidDTO createBidDTO) {
            try {
                if (createBidDTO == null)
                    return MessageResponse<bool>.Success ("Datos de puja vacíos.", false);

                Entities.AuctionsProduct? currentAuction = await _context.AuctionsProducts
                    .Where (a => a.Id == createBidDTO.AuctionId)
                    .FirstOrDefaultAsync ();
                if (currentAuction == null)
                    return MessageResponse<bool>.Success ("Subasta no encontrada.", false);
                if (currentAuction?.StatusId == 2)
                    return MessageResponse<bool>.Success ("Subasta pausada, no es posible realizar pujas.", false);
                if (currentAuction?.StatusId == 3)
                    return MessageResponse<bool>.Success ("Subasta cancelada, no es posible realizar pujas.", false);

                Entities.BidsAuction newBid = new Entities.BidsAuction ();
                newBid.BuyerId = createBidDTO.BuyerId;
                newBid.AuctionId = createBidDTO.AuctionId;

                _context.BidsAuctions.Add (newBid);
                await _context.SaveChangesAsync ();

                return MessageResponse<bool>.Success ("Puja registrada correctamente.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
