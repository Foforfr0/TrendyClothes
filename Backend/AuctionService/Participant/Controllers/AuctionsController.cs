using AuctionParticipantService.Models;
using AuctionParticipantService.DAO;
using AuctionParticipantService.Models.Consult;
using AuctionParticipantService.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionParticipantService.Controllers
{
    [ApiController]
    [Route("api/Auction/Participant/Auctions")]
    public class AuctionsController : Controller
    {
        private readonly IConsultAuctionService _consultAuctionService;

        public AuctionsController(IConsultAuctionService consultAuctionService)
        {
            _consultAuctionService = consultAuctionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAuctions()
        {
            try
            {
                MessageResponse<List<AuctionFullDTO>> response = await _consultAuctionService.GetActiveAuctionsAsync();

                if (response.IsError)
                    return HttpResponses.InternalServerError(response.Message);

                if (response.DataRetrieved == null || response.DataRetrieved.Count == 0)
                    return NotFound(new { response.Message });

                return Ok(new
                {
                    response.Message,
                    body = response.DataRetrieved
                });
            }
            catch (Exception ex)
            {
                return HttpResponses.InternalServerError(ex.ToString());
            }
        }
    }
}
