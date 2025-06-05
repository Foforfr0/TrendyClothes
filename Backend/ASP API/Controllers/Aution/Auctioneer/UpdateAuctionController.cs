using Backend.DTO;
using Backend.DTO.Auction.Update;
using Backend.Services.Intefaces.Auction;
using Backend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Aution.Auctioneer {
    [ApiController]
    [Authorize]
    [Route("api/Auction/Auctioneer/[controller]")]
    public class UpdateAuctionController : Controller {
        private readonly IUpdateAuctionService _updateAuctionService;

        public UpdateAuctionController (IUpdateAuctionService updateAuctionService) {
            _updateAuctionService = updateAuctionService;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAuction ([FromBody] UpdateStatusDTO updateAuctionDTO) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest(new {
                        message = "Los datos de la subasta a actualizar son inválidos.",
                        error = ModelState.GetErrors()
                    });
                MessageResponse<bool> response = await _updateAuctionService.UpdateStatusAsync(updateAuctionDTO);
                if (response.IsError)
                    return HttpResponses.InternalServerError(response.Message);
                if (!response.DataRetrieved)
                    return Conflict(new { response.Message });
                return Ok(new { response.Message });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError(ex.Message);
            }
        }
    }
}
