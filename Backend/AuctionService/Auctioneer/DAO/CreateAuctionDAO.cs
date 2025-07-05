using AuctionAuctioneerService.Entities;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Create;

namespace AuctionAuctioneerService.DAO {
    public class CreateAuctionDAO {
        private readonly TrendyClothesDBContext _context;
        private readonly ConsultUserDAO _consultUserDAO;

        public CreateAuctionDAO (TrendyClothesDBContext context, ConsultUserDAO consultUserDAO) {
            _context = context;
            _consultUserDAO = consultUserDAO;
        }

        public async Task<MessageResponse<bool>> PostAuctionAsync (CreateAuctionDTO createAuctionDTO) {
            try {
                int idUser = await _consultUserDAO.GetIdUserFromUsername (createAuctionDTO.SellerUsername);

                if (createAuctionDTO == null)
                    return MessageResponse<bool>.Success ("Datos de subasta vacíos.", false);
                Entities.AuctionsProduct newAuction = new Entities.AuctionsProduct ();
                newAuction.Name = createAuctionDTO.Name;
                newAuction.FirstPrice = createAuctionDTO.FirstPrice;
                newAuction.Bid = createAuctionDTO.Bid ?? 0;
                newAuction.LastPrice = createAuctionDTO.FirstPrice;
                newAuction.DateStart = createAuctionDTO.DateStart ?? DateTime.Now;
                newAuction.DateEnd = createAuctionDTO.DateEnd ?? DateTime.Now.AddDays (1);
                newAuction.SellerId = idUser;
                newAuction.StatusId = createAuctionDTO.StatusId ?? 1;

                _context.AuctionsProducts.Add (newAuction);
                await _context.SaveChangesAsync ();

                return MessageResponse<bool>.Success ("Subasta registrada correctamente.", true);
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
