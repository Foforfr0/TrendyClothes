using AuctionParticipantService.DAO;
using AuctionParticipantService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionParticipantService.Controllers
{
    [ApiController]
    [Route("api/Auctions/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDAO _auctionsDAO;

        public AuctionController(AuctionDAO auctionsDAO)
        {
            _auctionsDAO = auctionsDAO;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAuctionsWithPhoto()
        {
            var response = await _auctionsDAO.GetActiveAuctionsWithPhotoAsync();

            if (response.IsError)
            {
                return HttpResponses.InternalServerError(response.Message);
            }

            if (response.DataRetrieved == null || response.DataRetrieved.Count == 0)
            {
                return NotFound(new { error = true, message = "No se encontraron subastas activas con foto.", body = new List<AuctionDTO>() });
            }

            return Ok(new
            {
                error = false,
                message = response.Message,
                body = response.DataRetrieved
            });
        }
    }
}
