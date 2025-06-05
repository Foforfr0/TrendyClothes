using Backend.DAO.Auction;
using Backend.DTO;
using Backend.DTO.Auction.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Aution.Participant {
    [ApiController]
    [Authorize]
    [Route ("api/Auction/Participant/[controller]")]
    public class CreateBidController : Controller {
        private readonly CreateAuctionDAO _createAuctionDAO;

        public CreateBidController (CreateAuctionDAO createAuctionDAO) {
            _createAuctionDAO = createAuctionDAO;
        }

        [HttpPost]
        public async Task<IActionResult> PostBid ([FromBody] CreateBidDTO createBidDTO) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest ($"Datos recibidos inválidos. {ModelState}");
                MessageResponse<bool> response = await _createAuctionDAO.PostBidAsync (createBidDTO);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved == false)
                    return NotFound (new {
                        response.Message
                    });
                return Ok (new {
                    response.Message
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
