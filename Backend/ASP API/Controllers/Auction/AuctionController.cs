using Backend.DTO.Auction;
using Backend.Services.Intefaces.Auction;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Auction
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : Controller
    {        
            private readonly IAuctionService _auctionService;

            public AuctionController(IAuctionService auctionService)
            {
                _auctionService = auctionService;
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<AuctionDTO>> GetAuction(int id)
            {
                var auction = await _auctionService.GetAuctionAsync(id);
                if (auction == null) return NotFound();
                return Ok(auction);
            }

            [HttpPost("bid")]
            public async Task<ActionResult> PlaceBid(BidDTO bid)
            {
                var success = await _auctionService.SubmitBidAsync(bid);
                if (!success) return BadRequest("La puja no fue válida.");
                return Ok("Puja aceptada.");
            }
        }

    }
}
