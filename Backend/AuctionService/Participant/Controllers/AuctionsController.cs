using AuctionParticipantService.Models;
using AuctionParticipantService.Models.Consult;
using AuctionParticipantService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionParticipantService.Controllers
{
    [ApiController]
    [Route("api/Auction/Participant/Auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionParticipantService _service;

        public AuctionsController(IAuctionParticipantService service)
        {
            _service = service;
        }

        [HttpGet("Active")]
        public async Task<ActionResult<MessageResponse<List<AuctionFullDTO>>>> GetActiveAuctions()
        {
            var result = await _service.GetActiveAuctionsAsync();
            return Ok(result);
        }
    }
}
