using Backend.DTO;
using Backend.DTO.Auction.Create;
using Backend.Services.Intefaces.Auction;
using Backend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Aution.Auctioneer {
    [ApiController]
    [Authorize]
    [Route ("api/Auction/Auctioneer/[controller]")]
    public class CreateAuctionController : Controller {
        private readonly ICreateAuctionService _createAuctionService;

        public CreateAuctionController (ICreateAuctionService createAuctionService) {
            _createAuctionService = createAuctionService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAuction ([FromBody] CreateAuctionDTO createAuctionDTO) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest (new {
                        message = $"Los datos de la nueva subasta son inválidos.",
                        error = ModelState.GetErrors ()
                    });
                MessageResponse<bool> response = await _createAuctionService.CreateAuctionAsync (createAuctionDTO, User.Identity.Name);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (!response.DataRetrieved)
                    return Conflict (new {
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
